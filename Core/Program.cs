using Core.Controllers;
using Ryadel.Components.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class Program
    {
        static bool logged = false;
        public static int loggerUserID;
        static string actualNickname;


        static string[] opciones;
        static string[] opcionesCrud;

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
                    Usuario_IdPerfil = 3,
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
                        Console.WriteLine($"Se ha iniciado sesión correctamente! Usuario: {username}");

                        logged = true;
                        actualNickname = username;
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

                    int idPerfil = hospital.Usuarios.Where(
                                user => user.Usuario_Nickname == actualNickname
                            ).First().Usuario_IdPerfil;

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
                                "Manejar Tipos de Documentos",
                                "Manejar Tipos de NCF",
                                "Manejar Tipos de Pagos",
                                "Manejar Tipos de Personas",
                                "Manejar Tipos de Processo",
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
                                "Manejar Tipos de Documentos",
                                "Manejar Tipos de NCF",
                                "Manejar Tipos de Pagos",
                                "Manejar Tipos de Personas",
                                "Manejar Tipos de Processo",
                                "Manejar Usuarios",
                                "Exit"
                            };
                            break;
                        case 4: // Paciente
                                // ReciboIngreso. 1.Crear 2. Leer
                                // Pagos 1. Crear 2. Leer
                                // Cuenta Factura 1. Leer
                                // Persona 1. Crear 2. Leer
                                // Plan de Tratamiento 1. Leer
                                // ProcesoMedico 1. Leer
                                // Usuarios 1.Leer
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
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.WriteLine(".-      Acciones       -.");

            try
            {
                hospitalEntities hospital = new hospitalEntities();

                int idPerfil = hospital.Usuarios.Where(
                            user => user.Usuario_Nickname == actualNickname
                        ).First().Usuario_IdPerfil;

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
            string opcion;

            do
            {
                // Instanciar clase controller en cada accion

                opcion = Menu();

                string accion;

                Console.Clear();

            } while (opcion != "Exit");

            NLog.LogManager.Shutdown();
        }
    }
}
