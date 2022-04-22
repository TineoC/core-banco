using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Funciona
namespace Core.Controllers
{
   internal class PerfilController
    {
        static hospitalEntities hospital = new hospitalEntities();
        public static void MostrarInformacion(Perfil perfil)
        {
            Console.WriteLine($"Identificacion: {perfil.Perfil_Id}");
            Console.WriteLine($"Descripcion: {perfil.Perfil_Descripcion}");
            Console.WriteLine($"Fecha Creación: {perfil.Perfil_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {perfil.Perfil_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {perfil.Perfil_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
               bool exists = true;
                string descripcion;
                do
                {
                    
                    Console.Write("Escribe la descripcion del perfil ");
                    descripcion = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Perfil.Any(perfil => perfil.Perfil_Descripcion == descripcion);

                    if (exists)
                    {
                        Console.WriteLine("Existe un perfil con esa descripcion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);



                hospital.Perfil.Add(new Perfil()
                {
                    Perfil_Descripcion = descripcion,
                    Perfil_FechaCreacion = DateTime.Now,
                    Perfil_IdUsuarioCreador = Program.loggerUserID,
                    Perfil_Vigencia = true
                }); ;

                Logger.Info($"Se ha creado la Perfil correctamente");

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
            int perfilId;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el ID del Perfil a mostrar: ");
                perfilId = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Perfil.Any(per => per.Perfil_Id == perfilId);

                if (!exists)
                {
                    Console.WriteLine("No existe un  Perfil con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            Perfil Perfil = hospital.Perfil
                            .Where(
                                per => per.Perfil_Id == perfilId
                            )
                            .FirstOrDefault();

            MostrarInformacion(Perfil);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (Perfil perfil in hospital.Perfil.ToList())
            {
                Console.Clear();
                Console.WriteLine($"Perfil: {index}");

                MostrarInformacion(perfil);
                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int perfilid;
            string descripcion;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el ID del Perfil a actualizar: ");
                perfilid = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Perfil.Any(pers => pers.Perfil_Id == perfilid);

                if (!exists)
                {
                    Console.WriteLine("No existe un  Perfil con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {
                exists = true;
                Console.Write("Escribe la descripcion del perfil (actualizada) ");
                descripcion = Console.ReadLine();

                Console.Clear();

                exists = hospital.Perfil.Any(perfil => perfil.Perfil_Descripcion == descripcion);

                if (exists)
                {
                    Console.WriteLine("Existe un perfil con esa descripcion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);



            Perfil nuevoPerfil = hospital.Perfil.Where(
                    pers => pers.Perfil_Id == perfilid
                ).First();

            nuevoPerfil.Perfil_Descripcion = descripcion;
       

            Logger.Info($"La Perfil con el ID {perfilid} ha sido actualizado.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int perfilid;

                do
                {
                    Console.Write("Escribe el ID del Perfil a eliminar: ");
                    perfilid = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Perfil.Any(pers => pers.Perfil_Id == perfilid);

                    if (!exists)
                    {
                        Console.WriteLine("No existe un perfil  con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);


                hospital.Perfil.Where(
                        perfil => perfil.Perfil_Id == perfilid
                ).First().Perfil_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"El Perfil con el ID: {perfilid} ha sido eliminado.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
