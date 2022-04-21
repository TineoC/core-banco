using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Funciona
namespace Core.Controllers
{
    internal class UsuariosController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(Usuarios usuario)
        {
            Console.WriteLine($"Nickname: {usuario.Usuario_Nickname}");
            Console.WriteLine($"Contraseña: {usuario.Usuario_Contraseña}");
            Console.WriteLine($"ID del perfil: {usuario.Usuario_IdPerfil}");
            Console.WriteLine($"ID de la persona: {usuario.IdPersona}");
            Console.WriteLine($"Fecha Creación: {usuario.Usuario_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {usuario.Usuario_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {usuario.Usuario_Vigencia}");
        }

        public static void Crear()
        {
            
            var Logger = NLog.LogManager.GetCurrentClassLogger();
            
            Console.Clear();

            try
            {

                string nickname, password,persona;
                int perfil;
                bool exists= true;

                do
                { 
                    

                    Console.Write("Escribe tu nickname: ");
                    nickname = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Usuarios.Any(user => user.Usuario_Nickname == nickname); //??

                    if (exists)
                    {
                        Console.WriteLine("Existe un usuario con ese nickname");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);

                do
                {

                    Console.Write("Escribe la contraseña: ");
                    password = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Usuarios.Any(user => user.Usuario_Contraseña == password); //??

                    if (exists)
                    {
                        Console.WriteLine("Existe un usuario con la misma contraseña");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);

                 exists = false;
                do
                {

                    Console.Write("Escribe el ID del paciente: ");
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

                    Console.WriteLine("Escribe el ID del perfil: ");
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


                hospital.Usuarios.Add(new Usuarios()
                {
                    Usuario_Nickname = nickname,
                    Usuario_Contraseña = password,
                    Usuario_IdPerfil = perfil,
                    IdPersona = persona,
                });

                Logger.Info($"Se ha creado un usuarios correctamente con el nickname {nickname}");

                hospital.SaveChanges();
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
            string nickname,password;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el nickname del usuario a mostrar: ");
                nickname = Console.ReadLine();
                Console.Clear();

                exists = hospital.Usuarios.Any(user => user.Usuario_Nickname == nickname);

                if (!exists)
                {
                    Console.WriteLine("No existen usuarios con ese nickname");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);



            do
            {
                Console.Write("Escribe la contraseña del usuario a mostrar: ");
                password = Console.ReadLine();
                Console.Clear();

                exists = hospital.Usuarios.Any(user => user.Usuario_Contraseña == password);

                if (!exists)
                {
                    Console.WriteLine("No existen usuarios con esa contraseña");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();


            Usuarios usuario = hospital.Usuarios
                       .Where(
                           user => user.Usuario_Nickname == nickname
                       )
                       .FirstOrDefault();

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
        public static void Actualizar()
        {
            string nickname, password, persona;
            int perfil;
            bool exists = true;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {


                Console.Write("Escribe el nickname (actualizado): ");
                nickname = Console.ReadLine();

                Console.Clear();

                exists = hospital.Usuarios.Any(user => user.Usuario_Nickname == nickname); //??

                if (exists)
                {
                    Console.WriteLine("Existe un usuario con ese nickname");

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);

            do
            {

                Console.WriteLine("Escribe la contraseña (actualizada): ");
                password = Console.ReadLine();

                Console.Clear();

                exists = hospital.Usuarios.Any(user => user.Usuario_Contraseña == password); //??

                if (exists)
                {
                    Console.WriteLine("Existe un usuario con la misma contraseña");

                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);

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

            Usuarios usuario = hospital.Usuarios
                            .Where(
                                user => user.Usuario_Nickname == nickname
                            )
                            .FirstOrDefault();


            Usuarios nuevoUsuarios = hospital.Usuarios.Where(
                    user => user.Usuario_Nickname == nickname
                ).First();

            nuevoUsuarios.Usuario_Nickname = nickname;
            nuevoUsuarios.Usuario_Contraseña = password;
            nuevoUsuarios.Usuario_IdPerfil = perfil;
            nuevoUsuarios.IdPersona = persona;


            Logger.Info($"El usuario con el nickname {nickname} ha sido actualizado.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                string nickname, password, persona;
                int perfil;
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

                do
                {

                    Console.WriteLine("Escribe la contraseña a eliminar: ");
                    password = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Usuarios.Any(user => user.Usuario_Contraseña == password); //??

                    if (!exists)
                    {
                        Console.Write("No existe un usuario con la misma contraseña");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

      

                hospital.Usuarios.Remove(hospital.Usuarios.Where(
                        pers => pers.Usuario_Nickname == nickname 
                    ).First()
                );

                hospital.SaveChanges();

                Logger.Info($"El usuario con el nickname {nickname} ha sido eliminado.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
