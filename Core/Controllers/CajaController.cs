using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class CajaController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(Caja caja)
        {
            Console.WriteLine($"ID caja: {caja.Caja_Id}");
            Console.WriteLine($"Descripción: {caja.Caja_Descripcion}");
            Console.WriteLine($"Fecha Creación: {caja.Caja_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {caja.Caja_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {caja.Caja_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.WriteLine("Descripción de Caja: ");
                string descripcion = Console.ReadLine();

                hospital.Caja.Add(new Caja()
                {
                    Caja_Descripcion = descripcion,
                    Caja_FechaCreacion = DateTime.Now,
                    Caja_IdUsuarioCreador = Program.loggerUserID,
                    Caja_Vigencia = true
                });

                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Caja correctamente");
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

            try
            {
                do
                {
                    Console.Write("Escribe el id de la Caja: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(caj => caj.Caja_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                Caja caja = hospital.Caja
                                .Where(
                                    caj => caj.Caja_Id == id
                                )
                                .FirstOrDefault();

                MostrarInformacion(caja);
            }

            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }
        }
        public static void MostrarTodos()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                int index = 1;
                foreach (Caja caja in hospital.Caja.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Caja: #{index}");

                    MostrarInformacion(caja);

                    index++;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int id;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                do
                {
                    Console.Write("Escribe el id de la Caja: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(caj => caj.Caja_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Descripción de Caja (actualizado): ");
                string descripcion = Console.ReadLine();

                Caja nuevaCaja = hospital.Caja
                    .Where(
                        caj => caj.Caja_Id == id
                    ).First();

                nuevaCaja.Caja_Descripcion = descripcion;
                nuevaCaja.Caja_Vigencia = true;

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la Caja correctamente.");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }    

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
                    Console.Write("Escribe el id de la Caja: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(caj => caj.Caja_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.Caja.Where(
                    caj => caj.Caja_Id == id
                    ).First().Caja_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado esta Cajaf correctamente.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }
    }
}
