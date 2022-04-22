using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
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

        public static TipoPagoController Instancia = null;
        public static TipoPagoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new TipoPagoController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(TipoPago tipoPago)
        {
            Console.WriteLine($"Identificacion del tipo de Persona: {tipoPago.TipoPago_Id}");
            Console.WriteLine($"Descripcion del tipo de Pago: {tipoPago.TipoPago_Descripcion}");
            Console.WriteLine($"Fecha de creacion: { tipoPago.TipoPago_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: { tipoPago.TipoPago_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: { tipoPago.TipoPago_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {

                TipoPago TipoPago = new TipoPago()
                {
                    TipoPago_Id = 0,
                    TipoPago_Descripcion = descripcion,
                    TipoPago_FechaCreacion = DateTime.Now,
                    TipoPago_IdUsuarioCreador = Program.loggerUserID,
                    TipoPago_Vigencia = true
                };
                bool exists = true;
                string descripcion;
                do
                {


                    Console.Write("Escribe la descripcion de tipo de pago: ");
                    descripcion = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.TipoPago.Any(tipopg => tipopg.TipoPago_Descripcion == descripcion);

                    if (exists)
                    {
                        Console.WriteLine("Existe un tipo de pago con esa descripcion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);



                hospital.TipoPago.Add(new TipoPago()
                {
                    TipoPagoId = TipoPago.TipoPago_Id,
                    TipoPagoDescripcion = TipoPago.TipoPago_Descripcion,
                    TipoPagoFechaCreacion = TipoPago.TipoPago_FechaCreacion,
                    TipoPagoIdUsuarioCreador = TipoPago.TipoPago_IdUsuarioCreador,
                    TipoPagoVigencia = true,
                    EntidadId = 18
                };

                hospital.TipoPago.Add(TipoPago);


                Logger.Info($"Se ha creado el tipo de Pago correctamente: {descripcion}");
                hospital.SaveChanges();

                await SendMessageQueue(TipoPagoEntities);
                Logger.Info($"El tipo de pago {TipoPagoEntities.TipoPagoDescripcion} se ha enviado correctamente");
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
                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int TipoPago;
            string descripcion;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de Pago  a actualizar: ");
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

            do
            {
                exists = true;
                Console.Write("Escribe la descripcion del Pago (actualizado): ");
                 descripcion = Console.ReadLine();

                Console.Clear();

                exists = hospital.TipoPago.Any(tipoPg => tipoPg.TipoPago_Descripcion == descripcion);

                if (exists)
                {
                    Console.WriteLine("Existen tipos de pago con esa descripcion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);

   


            TipoPago nuevoTipoPago = hospital.TipoPago.Where(
                    tipoPg => tipoPg.TipoPago_Id == TipoPago
                ).First();

            nuevoTipoPago.TipoPago_Descripcion = descripcion;

            TipoPagoEntities TipoPagoEntities = new TipoPagoEntities()
            {
                TipoPagoId = nuevoTipoPago.TipoPago_Id,
                TipoPagoDescripcion = nuevoTipoPago.TipoPago_Descripcion,
                TipoPagoFechaCreacion = nuevoTipoPago.TipoPago_FechaCreacion,
                TipoPagoIdUsuarioCreador = nuevoTipoPago.TipoPago_IdUsuarioCreador,
                TipoPagoVigencia = true,
                EntidadId = 18
            };


            Logger.Info($"El tipo de Pago con la identificacion {TipoPago} ha sido actualizado.");

            hospital.SaveChanges();

            await SendMessageQueue(TipoPagoEntities);
            Logger.Info($"El tipo de pago {TipoPagoEntities.TipoPagoDescripcion} se ha enviado correctamente");
        }
        public async Task Eliminar()
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
                        Console.WriteLine("No existen tipos de Pagos con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                TipoPago nuevoTipoPago = hospital.TipoPago.Where(
                        tipoPg => tipoPg.TipoPago_Id == TipoPago
                    ).First();

                nuevoTipoPago.TipoPago_Vigencia = false;

                TipoPagoEntities TipoPagoEntities = new TipoPagoEntities()
                {
                    TipoPagoId = nuevoTipoPago.TipoPago_Id,
                    TipoPagoDescripcion = nuevoTipoPago.TipoPago_Descripcion,
                    TipoPagoFechaCreacion = nuevoTipoPago.TipoPago_FechaCreacion,
                    TipoPagoIdUsuarioCreador = nuevoTipoPago.TipoPago_IdUsuarioCreador,
                    TipoPagoVigencia = nuevoTipoPago.TipoPago_Vigencia,
                    EntidadId = 18
                };

                hospital.SaveChanges();

                Logger.Info($"El tipo de Pago con la identifiacion {TipoPago} ha sido eliminado.");


                await SendMessageQueue(TipoPagoEntities);
                Logger.Info($"El tipo de pago {TipoPagoEntities.TipoPagoDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(TipoPagoEntities TipoPagoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(TipoPagoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
