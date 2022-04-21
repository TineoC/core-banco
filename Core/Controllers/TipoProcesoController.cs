using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//TipoProcesoController ??? Para Hacer Operaciones
// Funciona puede mejorar
namespace Core.Controllers
{
    internal class TipoProcesoController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(TipoProceso tipoproceso)
        {
            Console.WriteLine($"Identificacion del tipo de proceso: {tipoproceso.TipoProceso_Id}");
            Console.WriteLine($"Descripcion del tipo de proceso: {tipoproceso.TipoProceso_Descripcion}");
            Console.WriteLine($"Fecha de creacion: { tipoproceso.TipoProceso_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: { tipoproceso.TipoProceso_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: { tipoproceso.TipoProceso_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe la descripcion de tipo de proceso: ");
                string descripcion = Console.ReadLine();



                hospital.TipoProceso.Add(new TipoProceso()
                {
                    TipoProceso_Descripcion = descripcion
                });


                Logger.Info($"Se ha creado el tipo de proceso correctamente: {descripcion}");
                

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
            int TipoProceso;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de proceso: ");
                TipoProceso = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoProceso.Any(tipopro => tipopro.TipoProceso_Id == TipoProceso);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de procesos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            TipoProceso tipoproceso = hospital.TipoProceso
                            .Where(
                                tipopro => tipopro.TipoProceso_Id == TipoProceso
                            )
                            .FirstOrDefault();

            MostrarInformacion(tipoproceso);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (TipoProceso tipoproceso in hospital.TipoProceso.ToList())
            {
                Console.Clear();
                Console.WriteLine($"TipoProceso: {index}");

                MostrarInformacion(tipoproceso);

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int TipoProceso;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la descripcion del tipo de proceso  a actualizar: ");
                TipoProceso = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoProceso.Any(tipopro => tipopro.TipoProceso_Id == TipoProceso);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de procesos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe la descripcion del proceso (actualizado): ");
            string descripcion = Console.ReadLine();

            
            TipoProceso nuevoTipoProceso = hospital.TipoProceso.Where(
                    tipopro => tipopro.TipoProceso_Id == TipoProceso
                ).First();

            nuevoTipoProceso.TipoProceso_Descripcion = descripcion;
         

            Logger.Info($"El tipo de proceso con la identificacion {TipoProceso} ha sido actualizado.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int TipoProceso;

                do
                {
                    Console.Write("Escribe la identifiacion del tipo de proceso a eliminar: ");
                    TipoProceso = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoProceso.Any(tipopro => tipopro.TipoProceso_Id == TipoProceso);

                    if (!exists)
                    {
                        Console.WriteLine("No existen tipos de procesos con esa identifiacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.TipoProceso.Remove(hospital.TipoProceso.Where(
                        tipopro => tipopro.TipoProceso_Id == TipoProceso
                    ).First()
                );

                hospital.SaveChanges();

                Logger.Info($"El tipo de proceso con la identifiacion {TipoProceso} ha sido eliminado.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }

}
