using Core.Controllers;
using Ryadel.Components.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class Program
    {
        static bool logged = false;
        public static int loggerUserID;

        /*
            1. Manejar los usuarios de la app
                1. Logear
                2. Registrarse
                3. Tener roles
            2. Crear, Actualizar, Eliminar y Mostrar
                1. Todas las entidades de la base de datos
                2. Limitar a que usuarios pueden acceder a las entidades
            3. Menu
                1. Con las acciones que puedes hacer en base a tu rol.
                    1. Hacer una cita
                    2. Pagar una factura
         */

        static bool Authentication(string nickname, string password)
        {
            bool existsUsername, correctPassword = false;

            using (hospitalEntities hospital = new hospitalEntities())
            {
                // Se valida
                //      1. Que exista ese usuario con el nickname
                
                existsUsername = hospital.Usuarios.Any(user => user.Usuario_Nickname == nickname);

                //      2. Que ese nickname tenga asignada esa contraseña

                if(existsUsername)
                {
                    correctPassword =
                    Equals(
                        hospital.Usuarios
                            .Where(
                                user => user.Usuario_Nickname == nickname
                            )
                            .FirstOrDefault()
                            .Usuario_Contraseña,
                        password
                    );
                }

                // Retornar true or false
            }

            return existsUsername && correctPassword;
        }

        static void Register()
        {
            hospitalEntities hospital = new hospitalEntities();

            // Evitar claves simples

            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                string username, password, confirmPassword;
                bool exists;

                // Se pregunta por el usuario y la contraseña
                Console.WriteLine(".-      Register       -.");

                do
                {
                    Console.Write("Ingrese un nombre de usuario: ");
                    username = Console.ReadLine();

                    exists = hospital.Usuarios.Any(
                        user => user.Usuario_Nickname == username);

                    if (exists)
                    {
                        Logger.Error("Ya existe un usuario con ese nombre!");

                        Console.WriteLine("Ya existe un usuario con ese nombre!");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);

                Console.Clear();

                bool simplePassword = true, passwordMatch = true;

                do
                {
                    Console.Clear();

                    Console.Write("Ingrese una contraseña: ");
                    password = PasswordCheck.GetPassword();

                    PasswordStrength strength = PasswordCheck.GetPasswordStrength(password);

                    switch (strength)
                    {
                        case PasswordStrength.Strong:
                            simplePassword = false;
                            break;
                        case PasswordStrength.VeryStrong:
                            simplePassword = false;
                            break;
                        default:
                            simplePassword = true;
                            break;
                    }

                    if (simplePassword)
                    {
                        Logger.Error("La contraseña es muy simple!");

                        char condicion1 = PasswordCheck.HasMinimumLength(password, 8) ? 'X' : ' ';
                        char condicion2 = PasswordCheck.HasUpperCaseLetter(password) ? 'X' : ' '; ;
                        char condicion3 = PasswordCheck.HasLowerCaseLetter(password) ? 'X' : ' '; ;
                        char condicion4 = PasswordCheck.HasDigit(password) || PasswordCheck.HasSpecialChar(password) ? 'X' : ' ';

                        Console.WriteLine("\nSi tienen una [X] es porque esta condición ya está cumplida, de lo contrario, estará vacía.\n");
                        Console.WriteLine("Las contraseñas requieren:");
                        Console.WriteLine($"\t[{condicion1}] 1.Al menos 8 caracteres de longitud:");
                        Console.WriteLine($"\t[{condicion2}] 2.Al menos un caracter en mayúscula:");
                        Console.WriteLine($"\t[{condicion3}] 3.Al menos un caracter en minúscula:");
                        Console.WriteLine($"\t[{condicion4}] 4.Al menos un dígito o carácter especial:");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    } else
                    {
                        Console.Write("\nComfirmar contraseña: ");
                        confirmPassword = PasswordCheck.GetPassword();

                        passwordMatch = Equals(password, confirmPassword);

                        if (!passwordMatch)
                        {
                            Logger.Error("Las contraseñas no coinciden!");
                            Console.WriteLine("\nLas contraseñas no coinciden!");
                            Console.Write("Press any key to continue...");
                            Console.ReadKey();
                        }
                    }
                } while (simplePassword || !passwordMatch);

                Logger.Info($"Intento de registro username: {username}  password: {password}");

                hospital.Usuarios.Add(new Usuarios()
                {
                    Usuario_Nickname = username,
                    Usuario_Contraseña = password,
                    Usuario_FechaCreacion = DateTime.Now,
                    Usuario_Vigencia = true
                });

                hospital.SaveChanges();

                Logger.Info($"El usuario {username} ha sido registrado");
                Console.WriteLine($"El usuario {username} ha sido registrado");

                Console.Write("\nPress any key to continue...");
                Console.ReadKey();

            }
            catch (Exception e)
            {
                Logger.Error(e, " Ha ocurrido un error inesperado.");
                throw;
            }
        }

        static void Login()
        {
            hospitalEntities hospital = new hospitalEntities();

            // Evitar claves simples

            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool correctCredentials;
                do
                {
                    
                    // Se pregunta por el usuario y la contraseña
                    Console.WriteLine(".-      LOGIN       -.");

                    Console.Write("Ingrese el nombre de usuario: ");
                    string username = Console.ReadLine();
                    Console.Write("Ingrese la contraseña: ");
                    string password = PasswordCheck.GetPassword();

                    Logger.Info($"Intento de inicio de sesión username: {username}  password: {password}");

                    // Se validan las credenciales
                    correctCredentials = Authentication(username, password);
                    
                    if (correctCredentials)
                    {
                        Logger.Info($"Se ha iniciado sesión correctamente! Usuario: {username}");
                        logged = true;
                        loggerUserID = hospital.Usuarios
                            .Where(
                                user => user.Usuario_Nickname == username
                            )
                            .FirstOrDefault()
                            .Usuario_Id;
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Logger.Warn($"Credenciales incorrectas inténtelo de nuevo. Usuario: {username} Password: {password}");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                        Console.Clear();
                    }
                } while (!correctCredentials);
                
            }
            catch (Exception e)
            {
                Logger.Error(e, " Ha ocurrido un error inesperado.");
                throw;
            }          
        }

        static string Menu()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.WriteLine(".-      MENU       -.");

            string[] opciones;

            try
            {
                if (!logged)
                {
                    opciones = new string[3]
                    {
                    "Login usuario",
                    "Registrar usuario",
                    "Exit"
                    };
                }
                else
                {
                    opciones = new string[13]
                    {
                    "Manejar Personas",
                    "Manejar Pacientes",             
                    "Manejar Aseguradoras",
                    "Manejar Autorizaciones",                   
                    "Manejar Egresos",
                    "Manejar Facturas",
                    "Manejar Cuentas",
                    "Manejar Cuentas de Facturas",
                    "Manejar Detalles de Facturas",
                    "Manejar Cajas",
                    "Manejar Aperturas y Cierres de Caja",
                    "Manejar Usuarios de Caja",
                    "Exit"
                    };
                }

                for (int i = 0; i < opciones.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {opciones[i]}");
                }

                int index = 0;

                do
                {
                    Console.Write("Opción: ");
                    index = Int32.Parse(Console.ReadLine());
                } while (!(index > 0 && index <= opciones.Length));

                string opcionElegida = opciones[index - 1];

                Logger.Info($"Opción elegida: {opcionElegida}");

                return opcionElegida;
            }
            catch (FormatException wrongFormat)
            {
                Logger.Error(wrongFormat, " ERROR: Debes ingresar un número");
                Console.WriteLine("ERROR: Debes ingresar un número");
                throw;
            }
            catch (Exception error)
            {
                Logger.Error(error, " Ha ocurrido un error inesperado");
                Console.WriteLine(error.Message, " Ha ocurrido un error inesperado");
                throw;
            }
        }

        static string CRUDMenu()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.WriteLine(".-      Acciones       -.");

            string[] opciones;

            try
            {
                opciones = new string[6]
                    {
                    "Crear",
                    "Mostrar",
                    "Mostrar Todos",
                    "Actualizar",
                    "Eliminar",
                    "Exit"
                    };

                for (int i = 0; i < opciones.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {opciones[i]}");
                }

                int index = 0;

                do
                {
                    Console.Write("Opción: ");
                    index = Int32.Parse(Console.ReadLine());
                } while (!(index > 0 && index <= opciones.Length));

                string opcionElegida = opciones[index - 1];

                Logger.Info($"Opción elegida: {opcionElegida}");

                return opcionElegida;
            }
            catch (FormatException wrongFormat)
            {
                Logger.Error(wrongFormat, " ERROR: Debes ingresar un número");
                Console.WriteLine("ERROR: Debes ingresar un número");
                throw;
            }
            catch (Exception error)
            {
                Logger.Error(error, " Ha ocurrido un error inesperado");
                Console.WriteLine(error.Message, " Ha ocurrido un error inesperado");
                throw;
            }
        }

        static void Main(string[] args)
        {
            // Main code

            string opcion;

            do
            {

                opcion = Menu();

                string accion;

                Console.Clear();
                PersonasController personas = new PersonasController();
                UsuarioCajaController usuariosCaja = new UsuarioCajaController();
                switch (opcion)
                {
            // Instanciar clase controller en cada accion
                    case "Login usuario":
                        Login();
                        break;

                    case "Registrar usuario":
                        Register();
                        break;

                    case "Manejar Personas":
                        accion = CRUDMenu();

                        switch(accion)
                        {
                            case "Crear":
                                personas.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                PersonasController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                PersonasController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                personas.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                personas.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }

                        break;

                    case "Manejar Pacientes":
                        //accion = CRUDMenu();

                        //switch (accion)
                        //{
                        //    case "Crear":
                        //        PacientesController.Crear();
                        //        Console.Write("Press any key to continue...");
                        //        Console.ReadKey();
                        //        break;
                        //    case "Mostrar":
                        //        PacientesController.Mostrar();
                        //        Console.Write("Press any key to continue...");
                        //        Console.ReadKey();
                        //        break;
                        //    case "Mostrar Todos":
                        //        PacientesController.MostrarTodos();
                        //        Console.Write("Press any key to continue...");
                        //        Console.ReadKey();
                        //        break;
                        //    case "Actualizar":
                        //        PacientesController.Actualizar();
                        //        Console.Write("Press any key to continue...");
                        //        Console.ReadKey();
                        //        break;
                        //    case "Eliminar":
                        //        PacientesController.Eliminar();
                        //        Console.Write("Press any key to continue...");
                        //        Console.ReadKey();
                        //        break;
                        //    case "Exit":
                        //        break;
                        //}
                        break;

                    case "Manejar Facturas":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                FacturaEncabezadoController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                FacturaEncabezadoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                FacturaEncabezadoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                FacturaEncabezadoController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                FacturaEncabezadoController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Aseguradoras":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                AseguradoraController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                AseguradoraController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                AseguradoraController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                AseguradoraController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                AseguradoraController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Autorizaciones":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                AutorizacionController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                AutorizacionController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                AutorizacionController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                AutorizacionController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                AutorizacionController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Egresos":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                EgresoController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                EgresoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                EgresoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                EgresoController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                EgresoController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Cuentas":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                CuentasController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                CuentasController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                CuentasController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                CuentasController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                CuentasController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Cuentas de Facturas":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                FacturaCuentaController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                FacturaCuentaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                FacturaCuentaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                FacturaCuentaController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                FacturaCuentaController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Detalles de Facturas":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                FacturaDetalleController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                FacturaDetalleController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                FacturaDetalleController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                FacturaDetalleController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                FacturaDetalleController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Cajas":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                CajaController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                CajaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                CajaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                CajaController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                CajaController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Aperturas y Cierres de Caja":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                AperturaYCierreDeCajaController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                AperturaYCierreDeCajaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todas":
                                AperturaYCierreDeCajaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                AperturaYCierreDeCajaController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                AperturaYCierreDeCajaController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Usuarios de Caja":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                usuariosCaja.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                UsuarioCajaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                UsuarioCajaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                usuariosCaja.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                usuariosCaja.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;
                        
                    case "Exit":
                        Console.WriteLine("Hasta luego...");
                        System.Threading.Thread.Sleep(1000);
                        break;

                    default:
                        break;
                }

                Console.Clear();

            } while (opcion != "Exit");

            NLog.LogManager.Shutdown();
        }
    }
}
