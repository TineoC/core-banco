using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class AseguradoraController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(Aseguradora aseguradora)
        {
            Console.WriteLine($"ID: {aseguradora.Aseguraodra_Id}");
            Console.WriteLine($"Descripción: {aseguradora.Aseguradora_Descripcion}");
            Console.WriteLine($"Fecha Creación: {aseguradora.Aseguradora_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {aseguradora.Aseguradora_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {aseguradora.Aseguradora_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe el nombre de la aseguradora: ");
                string nombre = Console.ReadLine();

                hospital.Aseguradora.Add(new Aseguradora()
                {
                    Aseguradora_Descripcion = nombre,
                    Aseguradora_FechaCreacion = DateTime.Now,
                    Aseguradora_IdUsuarioCreador = Program.loggerUserID,
                    Aseguradora_Vigencia = true
                });

                Logger.Info($"Se ha creado la Aseguradora correctamente");

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
            int id;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el id de la Aseguradora: ");

                id = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Aseguradora.Any(aseg => aseg.Aseguraodra_Id == id);

                if (!exists)
                {
                    Console.WriteLine("No existen Aseguradoras con ese id");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            Aseguradora aseguradora = hospital.Aseguradora
                            .Where(
                                aseg => aseg.Aseguraodra_Id == id
                            )
                            .FirstOrDefault();

            MostrarInformacion(aseguradora);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (Aseguradora aseguradora in hospital.Aseguradora.ToList())
            {
                Console.Clear();
                Console.WriteLine($"Aseguradora: {index}");

                MostrarInformacion(aseguradora);

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int id;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el id de la Aseguradora: ");

                id = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Aseguradora.Any(aseg => aseg.Aseguraodra_Id == id);

                if (!exists)
                {
                    Console.WriteLine("No existen Aseguradoras con ese id");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe el nombre de la aseguradora: (actualizado)");
            string nombre = Console.ReadLine();

            Aseguradora aseguradora = hospital.Aseguradora
                .Where(
                    aseg => aseg.Aseguraodra_Id == id
                ).First();

            aseguradora.Aseguradora_Descripcion = nombre;
            aseguradora.Aseguradora_Vigencia = true;

            hospital.SaveChanges();

            Logger.Info($"Se ha actualizado la Aseguradora correctamente.");

        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int id;

                do
                {
                    Console.Write("Escribe el id de la Aseguradora: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Aseguradora.Any(aseg => aseg.Aseguraodra_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Aseguradoras con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.Aseguradora.Where(
                    aseg => aseg.Aseguraodra_Id == id
                    ).First().Aseguradora_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Aseguradora ID: {id} correctamente.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }
    }
}
