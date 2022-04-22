using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using Ryadel.Components.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class UsuariosController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static UsuariosController Instancia = null;
        public static UsuariosController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new UsuariosController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(Usuarios usuario)
        {
            Console.WriteLine($"ID: {usuario.Usuario_Id}");
            Console.WriteLine($"Nickname: {usuario.Usuario_Nickname}");
            Console.WriteLine($"Contraseña: {usuario.Usuario_Contraseña}");
            Console.WriteLine($"ID del perfil: {usuario.Usuario_IdPerfil}");
            Console.WriteLine($"ID de la persona: {usuario.IdPersona}");
            Console.WriteLine($"Fecha Creación: {usuario.Usuario_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {usuario.Usuario_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {usuario.Usuario_Vigencia}");
        }

        public async Task Crear()
        {
            
            var Logger = NLog.LogManager.GetCurrentClassLogger();
            Console.Clear();

            try
            {
                string username, password, confirmPassword, persona;
                int perfil;
                bool exists= true;

                do
                {
                    Console.Write("Ingrese un nombre de usuario: ");
                    username = Console.ReadLine();

                    exists = hospital.Usuarios.Any(
                        user => user.Usuario_Nickname == username);

                    if (exists)
                    {
                        Logger.Error($"Ya existe un usuario con ese Nombre: {username}");
                        Console.WriteLine($"Ya existe un usuario con ese nombre!");

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
                    }
                    else
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

                do
                {

                    Console.WriteLine("Escribe el ID del perfil: ");
                    perfil = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Perfil.Any(user => user.Perfil_Id == perfil);

                    if (!exists)
                    {
                        Logger.Error($"No existe un perfil con ese ID: {perfil}");
                        Console.WriteLine("No existe un perfil con ese ID");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                exists = false;
                do
                {

                    Console.Write("Escribe el Documento del Paciente: ");
                    persona = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(user => user.Persona_Documento == persona);///??

                    if (!exists)
                    {
                        Logger.Error($"No existe un  paciente con esa Identificación: {persona}");
                        Console.WriteLine("No existe un  paciente con esa identicacion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Usuarios Usuarios = new Usuarios()
                {
                    Usuario_Nickname = username,
                    Usuario_Contraseña = password,
                    Usuario_IdPerfil = perfil,
                    IdPersona = persona,
                    Usuario_FechaCreacion = DateTime.Now,
                    Usuario_IdUsuarioCreador = Program.loggerUserID,
                    Usuario_Vigencia = true
                };

                UsuarioEntities UsuarioEntities = new UsuarioEntities()
                {
                    UsuarioId = Usuarios.Usuario_Id,
                    UsuarioNickname = Usuarios.Usuario_Nickname,
                    UsuarioContraseña = Usuarios.Usuario_Contraseña,
                    UsuarioIdPerfil = Convert.ToInt32(Usuarios.Perfil),
                    IdPersona = Usuarios.IdPersona,
                    UsuarioFechaCreacion = Usuarios.Usuario_FechaCreacion,
                    UsuarioIdUsuarioCreador = Usuarios.Usuario_IdUsuarioCreador,
                    UsuarioVigencia = true,
                    EntidadId = 21
                };


                hospital.Usuarios.Add(Usuarios);

                Logger.Info($"Se ha creado un usuarios correctamente con el Nickname: {username}");

                hospital.SaveChanges();

                await SendMessageQueue(UsuarioEntities);
                Logger.Info($"El usuario {UsuarioEntities.UsuarioNickname} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }
        }
        public static void Mostrar()
        {
            bool exists = false;
            string nickname, password;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el nickname del usuario a mostrar: ");
                nickname = Console.ReadLine();
                Console.Clear();

                exists = hospital.Usuarios.Any(user => user.Usuario_Nickname == nickname);

                if (!exists)
                {
                    Logger.Error($"No existen usuarios con ese Nickname: {nickname}");
                    Console.WriteLine("No existen usuarios con ese nickname");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            bool auth;

            do
            {
                Console.Write("Escribe la contraseña del usuario a mostrar: ");
                password = PasswordCheck.GetPassword();

                Console.WriteLine();

                Console.Clear();

                auth = Program.Authentication(nickname, password);

                if (!auth)
                {
                    Logger.Error($"Credenciales incorrectas. {nickname}");
                    Console.WriteLine("Credenciales incorrectas.");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!auth);

            Console.Clear();

            Usuarios usuario = hospital.Usuarios
                       .Where(
                           user => user.Usuario_Nickname == nickname
                       )
                       .First();

            MostrarInformacion(usuario);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (Usuarios usuario in hospital.Usuarios.ToList())
            {
                Console.Clear();
                Console.WriteLine($"Usuarios: {index}");

                MostrarInformacion(usuario);

                index++;
            }
        }
        public async Task Actualizar()
        {
            string nickname, password, persona;
            int perfil;
            bool exists = true;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el nickname del usuario a actualizar: ");
                nickname = Console.ReadLine();

                Console.Clear();

                exists = hospital.Usuarios.Any(user => user.Usuario_Nickname == nickname);

                if (!exists)
                {
                    Logger.Error($"No existe un usuario con el Nickname: {nickname}");
                    Console.WriteLine("No existe un usuario con el Nickname: {nickname}");

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            string nuevoNickname;

            do
            {
                Console.Write("Escribe el nuevo nickname: ");
                nuevoNickname = Console.ReadLine();

                Console.Clear();

                exists = hospital.Usuarios.Any(user => user.Usuario_Nickname == nuevoNickname);

                if (!exists)
                {
                    Logger.Error($"No existe un usuario con el Nickname: {nuevoNickname}");
                    Console.WriteLine($"No existe un usuario con el Nickname: {nuevoNickname}");

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            string confirmPassword;
            bool simplePassword, passwordMatch = false;

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
                }
                else
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

            exists = false;
            do
            {

                Console.WriteLine("Escribe el ID del paciente (actualizado) : ");
                persona = Console.ReadLine();

                Console.Clear();

                exists = hospital.Persona.Any(user => user.Persona_Documento == persona);///??

                if (!exists)
                {
                    Console.WriteLine("No existe un  paciente con esa identicacion");

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {
                Console.WriteLine("Escribe el ID del perfil (actualizado): ");
                perfil = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Perfil.Any(user => user.Perfil_Id == perfil);

                if (!exists)
                {
                    Console.WriteLine("No existe un perfil con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            Usuarios nuevoUsuarios = hospital.Usuarios.Where(
                    user => user.Usuario_Nickname == nickname
                ).First();

            nuevoUsuarios.Usuario_Nickname = nuevoNickname;
            nuevoUsuarios.Usuario_Contraseña = password;
            nuevoUsuarios.Usuario_IdPerfil = perfil;
            nuevoUsuarios.IdPersona = persona;
            nuevoUsuarios.Usuario_Vigencia = true;

            UsuarioEntities UsuarioEntities = new UsuarioEntities()
            {
                UsuarioId = nuevoUsuarios.Usuario_Id,
                UsuarioNickname = nuevoUsuarios.Usuario_Nickname,
                UsuarioContraseña = nuevoUsuarios.Usuario_Contraseña,
                UsuarioIdPerfil = Convert.ToInt32(nuevoUsuarios.Perfil),
                IdPersona = nuevoUsuarios.IdPersona,
                UsuarioFechaCreacion = nuevoUsuarios.Usuario_FechaCreacion,
                UsuarioIdUsuarioCreador = nuevoUsuarios.Usuario_IdUsuarioCreador,
                UsuarioVigencia = true,
                EntidadId = 21
            };


            Logger.Info($"El usuario con el nickname {nickname} ha sido actualizado.");

            hospital.SaveChanges();

            await SendMessageQueue(UsuarioEntities);
            Logger.Info($"El usuario {UsuarioEntities.UsuarioNickname} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                string nickname, password;
                bool exists = false;
              
                do
                {


                    Console.Write("Escribe el nickname a eliminar: ");
                    nickname = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Usuarios.Any(user => user.Usuario_Nickname == nickname); //??

                    if (!exists)
                    {
                        Console.Write("No existe un usuario con ese nickname");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                bool logged;

                do
                {

                    Console.WriteLine("Escribe la contraseña del usuario: ");
                    password = Console.ReadLine();

                    logged = Program.Authentication(nickname, password);

                    Console.Clear();

                    exists = hospital.Usuarios.Any(user => user.Usuario_Contraseña == password);

                    if (!logged)
                    {
                        Console.Write("Credenciales Incorrectas");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!logged);

                Usuarios nuevoUsuarios = hospital.Usuarios.Where(
                        pers => pers.Usuario_Nickname == nickname
                    ).First();

                nuevoUsuarios.Usuario_Vigencia = false;

                UsuarioEntities UsuarioEntities = new UsuarioEntities()
                {
                    UsuarioId = nuevoUsuarios.Usuario_Id,
                    UsuarioNickname = nuevoUsuarios.Usuario_Nickname,
                    UsuarioContraseña = nuevoUsuarios.Usuario_Contraseña,
                    UsuarioIdPerfil = Convert.ToInt32(nuevoUsuarios.Perfil),
                    IdPersona = nuevoUsuarios.IdPersona,
                    UsuarioFechaCreacion = nuevoUsuarios.Usuario_FechaCreacion,
                    UsuarioIdUsuarioCreador = nuevoUsuarios.Usuario_IdUsuarioCreador,
                    UsuarioVigencia = nuevoUsuarios.Usuario_Vigencia,
                    EntidadId = 21
                };


                hospital.SaveChanges();

                Logger.Info($"El usuario con el nickname {nickname} ha sido eliminado.");

                await SendMessageQueue(UsuarioEntities);
                Logger.Info($"El usuario {UsuarioEntities.UsuarioNickname} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(UsuarioEntities UsuarioEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(UsuarioEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
