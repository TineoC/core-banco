using Core.Controllers;
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
                    string password = Console.ReadLine();

                    Logger.Info($"Intento de inicio de sesión username: {username}  password: {password}");

                    // Se validan las credenciales
                    correctCredentials = Authentication(username, password);

                    if (correctCredentials)
                    {
                        Logger.Info($"Se ha iniciado sesión correctamente! Usuario: {username}");
                        logged = true;
                        Console.Write("Press any key to continue...");
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
                    opciones = new string[4]
                    {
                    "Manejar Personas",
                    "Manejar Pacientes",
                    "Manejar Facturas",
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
            catch (Exception error)
            {
                Logger.Error(error, " Ha ocurrido un error inesperado");
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
            catch (Exception error)
            {
                Logger.Error(error, " Ha ocurrido un error inesperado");
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

                switch (opcion)
                {
                    case "Login usuario":
                        Login();
                        break;

                    case "Registrar usuario":
                        break;

                    case "Exit":
                        Console.WriteLine("Hasta luego...");
                        System.Threading.Thread.Sleep(1000);
                        break;

                    case "Manejar Personas":
                        accion = CRUDMenu();

                        switch(accion)
                        {
                            case "Crear":
                                PersonasController.Crear();
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
                                PersonasController.Actualizar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Eliminar":
                                PersonasController.Eliminar();
                                Console.Write("Press any key to continue...");
                                Console.ReadKey();
                                break;
                            case "Exit":
                                break;
                        }
                        break;

                    case "Manejar Pacientes":
                        break;

                    case "Manejar Facturas":
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
