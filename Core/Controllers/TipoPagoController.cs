using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//??? Para Hacer Operaciones
//Funciona
namespace Core.Controllers
{
    internal class TipoPagoController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(TipoPago tipoPago)
        {
            Console.WriteLine($"Identificacion del tipo de Persona: {tipoPago.TipoPago_Id}");
            Console.WriteLine($"Descripcion del tipo de Pago: {tipoPago.TipoPago_Descripcion}");
            Console.WriteLine($"Fecha de creacion: { tipoPago.TipoPago_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: { tipoPago.TipoPago_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: { tipoPago.TipoPago_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe la descripcion de tipo de Pago: ");
                string descripcion = Console.ReadLine();



                hospital.TipoPago.Add(new TipoPago()
                {
                    TipoPago_Descripcion = descripcion
                });


                Logger.Info($"Se ha creado el tipo de Pago correctamente: {descripcion}");


                hospital.SaveChanges();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }
        }
        /// <summary>
        /// ///////////////////////////////////////////////
        /// </summary>
        public static void Mostrar()
        {
            bool exists = false;
            int TipoPago;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de Pago: ");
                TipoPago = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoPago.Any(tipoPg => tipoPg.TipoPago_Id == TipoPago);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Pagos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            TipoPago tipoPago = hospital.TipoPago
                            .Where(
                                tipoPg => tipoPg.TipoPago_Id == TipoPago
                            )
                            .FirstOrDefault();

            MostrarInformacion(tipoPago);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (TipoPago tipoPago in hospital.TipoPago.ToList())
            {
                Console.Clear();
                Console.WriteLine($"TipoPago: {index}");

                MostrarInformacion(tipoPago);

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int TipoPago;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la descripcion del tipo de Pago  a actualizar: ");
                TipoPago = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoPago.Any(tipoPg => tipoPg.TipoPago_Id == TipoPago);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Pagos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe la descripcion del Pago (actualizado): ");
            string descripcion = Console.ReadLine();


            TipoPago nuevoTipoPago = hospital.TipoPago.Where(
                    tipoPg => tipoPg.TipoPago_Id == TipoPago
                ).First();

            nuevoTipoPago.TipoPago_Descripcion = descripcion;


            Logger.Info($"El tipo de Pago con la identificacion {TipoPago} ha sido actualizado.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int TipoPago;

                do
                {
                    Console.Write("Escribe la identifiacion del tipo de Pago a eliminar: ");
                    TipoPago = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoPago.Any(tipoPg => tipoPg.TipoPago_Id == TipoPago);

                    if (!exists)
                    {
                        Console.WriteLine("No existen tipos de Pagos con esa identifiacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.TipoPago.Remove(hospital.TipoPago.Where(
                        tipoPg => tipoPg.TipoPago_Id == TipoPago
                    ).First()
                );

                hospital.SaveChanges();

                Logger.Info($"El tipo de Pago con la identifiacion {TipoPago} ha sido eliminado.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
