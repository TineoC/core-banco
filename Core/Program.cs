using Core.Controllers;
using Ryadel.Components.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DTO;
using System.Threading.Tasks;
using Core.Consumers;
using NLog;

namespace Core
{
    class Program
    {
        static Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        static bool logged = false;
        public static int loggerUserID;
        static string actualNickname;

        static string[] opciones;
        static string[] opcionesCrud;

        public static bool Authentication(string nickname, string password)
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

                if (existsUsername && correctPassword)
                {
                    Logger.Info($"Se ha autenticado el usuario Usuario: {nickname}");
                } else
                {
                    Logger.Error($"Ha habido un error al autenticar el usuario Usuario: {nickname}");
                }
                
                // Retornar true or false
            }

            return existsUsername && correctPassword;
        }

        public static void Register()
        {
            hospitalEntities hospital = new hospitalEntities();

            // Evitar claves simples

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
                        Console.WriteLine("Error: La contraseña es muy simple!");

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

                UserRegister RegistroUsuario = new UserRegister();
                //Integracion

                Usuarios usuarios = new Usuarios()
                {
                    Usuario_Nickname = username,
                    Usuario_Contraseña = password,
                    Usuario_IdPerfil = 3,
                    Usuario_FechaCreacion = DateTime.Now,
                    Usuario_Vigencia = true
                };

                hospital.Usuarios.Add(usuarios);
                hospital.SaveChanges();

                //Integracion

                RegistroUsuario.Crear(usuarios);

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
                        Logger.Info($"\nSe ha iniciado sesión correctamente! Usuario: {username}");
                        Console.WriteLine($"\nSe ha iniciado sesión correctamente! Usuario: {username}");

                        logged = true;

                        actualNickname = username;

                        loggerUserID = hospital.Usuarios
                            .Where(
                                user => user.Usuario_Nickname == username
                            )
                            .First()
                            .Usuario_Id;

                        //Integracion con CajaTopic
                        var e = CajaTopicConsumer.GetInstance();
                        e.StartAsync();
                        //Integracion con WebTopic
                        var y = WebTopicConsumer.GetInstance();
                        y.StartAsync();
                        //Integracion con otro hospital
                        var z = CoreHospitalQueueConsumer.GetInstance();
                        z.StartAsync();
                        Console.Write("\nPress any key to continue...");
                        Console.ReadKey();
                        
                        Console.Clear();
                        return;
                    }
                    else
                    {
                        Logger.Warn($"Credenciales incorrectas inténtelo de nuevo. Usuario: {username}");
                        Console.WriteLine("\nCredenciales incorrectas inténtelo de nuevo.");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();

                        Console.Clear();
                    }
                } while (!correctCredentials);
                
            }
            catch (Exception e)
            {
                Logger.Error(e, " Ha ocurrido un error inesperado.");
                Console.WriteLine(e.Message, " Ha ocurrido un error inesperado.");
                throw;
            }          
        }

        static string Menu()
        {
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
                    hospitalEntities hospital = new hospitalEntities();

                    int idPerfil = 
                        hospital.Usuarios.Where(
                                user => user.Usuario_Nickname == actualNickname
                            )
                        .First()
                        .Usuario_IdPerfil;

                    switch (idPerfil)
                    {
                        case 2: // Administrador
                            opciones = new string[24]
                            {
                                "Manejar Aperturas y Cierres de Caja",
                                "Manejar Aseguradoras",
                                "Manejar Autorizaciones",
                                "Manejar Cajas",
                                "Manejar Usuarios de Caja",
                                "Manejar Cuentas de Facturas",
                                "Manejar Cuentas",
                                "Manejar Egresos",
                                "Manejar Detalles de Facturas",
                                "Manejar Facturas",
                                "Manejar NCF",
                                "Manejar Pagos",
                                "Manejar Perfiles",
                                "Manejar Personas",
                                "Manejar Planes de Tratamiento",
                                "Manejar Procesos Médicos",
                                "Manejar Recibos de Ingreso",
                                "Manejar Tipos de Documento",
                                "Manejar Tipos de NCF",
                                "Manejar Tipos de Pagos",
                                "Manejar Tipos de Personas",
                                "Manejar Tipos de Procesos",
                                "Manejar Usuarios",
                                "Exit"
                            };
                            break;
                        case 3: // Soporte Técnico
                            opciones = new string[24]
                            {
                                "Manejar Aperturas y Cierres de Caja",
                                "Manejar Aseguradoras",
                                "Manejar Autorizaciones",
                                "Manejar Cajas",
                                "Manejar Usuarios de Caja",
                                "Manejar Cuentas de Facturas",
                                "Manejar Cuentas",
                                "Manejar Egresos",
                                "Manejar Detalles de Facturas",
                                "Manejar Facturas",
                                "Manejar NCF",
                                "Manejar Pagos",
                                "Manejar Perfiles",
                                "Manejar Personas",
                                "Manejar Planes de Tratamiento",
                                "Manejar Procesos Médicos",
                                "Manejar Recibos de Ingreso",
                                "Manejar Tipos de Documento",
                                "Manejar Tipos de NCF",
                                "Manejar Tipos de Pagos",
                                "Manejar Tipos de Personas",
                                "Manejar Tipos de Procesos",
                                "Manejar Usuarios",
                                "Exit"
                            };
                            break;
                        case 4: // Paciente
                            opciones = new string[9]
                            {
                                "Manejar Recibos de Ingreso",
                                "Manejar Pagos",
                                "Manejar Cuentas de Facturas",
                                "Manejar Cuentas",
                                "Manejar Personas",
                                "Manejar Planes de Tratamiento",
                                "Manejar Procesos Médicos",
                                "Manejar Usuarios",
                                "Exit"
                            };

                            break;
                        default:
                            break;
                    }
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
            Console.WriteLine(".-      Acciones       -.");

            try
            {
                hospitalEntities hospital = new hospitalEntities();

                int idPerfil = 
                    hospital.Usuarios.Where(
                            user => user.Usuario_Nickname == actualNickname
                        )
                    .First()
                    .Usuario_IdPerfil;

                switch (idPerfil)
                {
                    case 2:
                        opcionesCrud = new string[6]
                            {
                                "Crear",
                                "Mostrar",
                                "Mostrar Todos",
                                "Actualizar",
                                "Eliminar",
                                "Exit"
                            };
                        break;
                    case 3:
                        opcionesCrud = new string[4]
                            {
                                "Crear",
                                "Mostrar",
                                "Mostrar Todos",
                                "Exit"
                            };
                        break;
                    case 4:
                        opcionesCrud = new string[4]
                            {
                                "Crear",
                                "Mostrar",
                                "Mostrar Todos",
                                "Exit"
                            };
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < opcionesCrud.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {opcionesCrud[i]}");
                }

                int index = 0;

                do
                {
                    Console.Write("Opción: ");
                    index = Int32.Parse(Console.ReadLine());
                } while (!(index > 0 && index <= opcionesCrud.Length));

                string opcionElegida = opcionesCrud[index - 1];

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
                //Integracion
                PersonasController personas = new PersonasController();
                UsuarioCajaController usuariosCaja = new UsuarioCajaController();
                FacturaEncabezadoController facturaEncabezado = new FacturaEncabezadoController();
                FacturaDetalleController FacturaDetalle = new FacturaDetalleController();
                FacturaCuentaController facturaCuenta = new FacturaCuentaController();
                EgresoController egreso = new EgresoController();
                CuentasController cuenta = new CuentasController();
                CajaController caja = new CajaController();
                AutorizacionController autorizacion = new AutorizacionController();
                AseguradoraController aseguradora = new AseguradoraController();
                AperturaYCierreDeCajaController aperturaYCierreDeCaja = new AperturaYCierreDeCajaController();
                TipoProcesoController TipoProceso = new TipoProcesoController();
                UsuariosController usuariosController = new UsuariosController();
                PerfilController perfil1 = new PerfilController();
                PagoController pago = new PagoController();
                TipoPersonaController TipoPersona = new TipoPersonaController();
                TipoPagoController tipoPagos = new TipoPagoController();
                TipoDocumentoController TipoDocumentos = new TipoDocumentoController();
                TipoNCFController tipoNCF = new TipoNCFController();
                ProcesoMedicoController procesosMedicos = new ProcesoMedicoController();
                ReciboIngresoController RecibosIngresos = new ReciboIngresoController();
                PlanDeTratamientoController planTratamiento = new PlanDeTratamientoController();

                switch (opcion)
                {
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

                    case "Manejar Facturas":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                facturaEncabezado.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                FacturaEncabezadoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                FacturaEncabezadoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                facturaEncabezado.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                facturaEncabezado.Eliminar();
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
                                aseguradora.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                AseguradoraController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                AseguradoraController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                aseguradora.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                aseguradora.Eliminar();
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
                                autorizacion.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                AutorizacionController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                AutorizacionController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                autorizacion.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                autorizacion.Eliminar();
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
                                egreso.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                EgresoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                EgresoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                egreso.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                egreso.Eliminar();
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
                                cuenta.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                CuentasController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                CuentasController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                cuenta.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                cuenta.Eliminar();
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
                                facturaCuenta.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                FacturaCuentaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                FacturaCuentaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                facturaCuenta.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                facturaCuenta.Eliminar();
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
                                FacturaDetalle.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                FacturaDetalleController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                FacturaDetalleController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                FacturaDetalle.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                FacturaDetalle.Eliminar();
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
                                caja.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                CajaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                CajaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                caja.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                caja.Eliminar();
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
                                aperturaYCierreDeCaja.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                AperturaYCierreDeCajaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                AperturaYCierreDeCajaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                aperturaYCierreDeCaja.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                aperturaYCierreDeCaja.Eliminar();
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

                    case "Manejar Tipos de Procesos":
                        accion = CRUDMenu();
                        switch (accion)
                        {
                            case "Crear":
                                TipoProceso.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                TipoProcesoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                TipoProcesoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                TipoProceso.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                TipoProceso.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Usuarios":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                usuariosController.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                UsuariosController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                UsuariosController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                usuariosController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                usuariosController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;
                    case "Manejar Perfiles":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                perfil1.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                PerfilController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                PerfilController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                perfil1.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                perfil1.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;
                    case "Manejar Pagos":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                pago.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                PagoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                PagoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                pago.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                pago.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Tipos de Personas":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                TipoPersona.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                TipoPersonaController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                TipoPersonaController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                TipoPersona.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                TipoPersona.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Tipos de Pagos":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                tipoPagos.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                TipoPagoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                TipoPagoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                tipoPagos.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                tipoPagos.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Tipos de Documento":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                TipoDocumentos.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                TipoDocumentoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                TipoDocumentoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                TipoDocumentos.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                TipoDocumentos.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Procesos Medicos":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                procesosMedicos.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                ProcesoMedicoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                ProcesoMedicoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                procesosMedicos.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                procesosMedicos.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Recibos de Ingreso":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                RecibosIngresos.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                ReciboIngresoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                ReciboIngresoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                RecibosIngresos.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                RecibosIngresos.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;

                        }
                        break;
                    case "Manejar Planes de Tratamiento":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                planTratamiento.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                PlanDeTratamientoController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                PlanDeTratamientoController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                planTratamiento.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                planTratamiento.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Tipos de NCF":
                        accion = CRUDMenu();

                        switch (accion)
                        {
                            case "Crear":
                                tipoNCF.Crear();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar":
                                TipoNCFController.Mostrar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Mostrar Todos":
                                TipoNCFController.MostrarTodos();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Actualizar":
                                tipoNCF.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                tipoNCF.Eliminar();
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
