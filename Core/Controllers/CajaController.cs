using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
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

        public static CajaController Instancia = null;
        public static CajaController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new CajaController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(Caja caja)
        {
            Console.WriteLine($"ID caja: {caja.Caja_Id}");
            Console.WriteLine($"Descripción: {caja.Caja_Descripcion}");
            Console.WriteLine($"Fecha Creación: {caja.Caja_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {caja.Caja_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {caja.Caja_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.WriteLine("Descripción de Caja: ");
                string descripcion = Console.ReadLine();

                Caja caja = new Caja() {
                    Caja_Descripcion = descripcion,
                    Caja_FechaCreacion = DateTime.Now,
                    Caja_IdUsuarioCreador = Program.loggerUserID,
                    Caja_Vigencia = true
                };

                CajaEntities cajaEntities = new CajaEntities()
                {
                    CajaDescripcion = caja.Caja_Descripcion,
                    CajaFechaCreacion = caja.Caja_FechaCreacion,
                    CajaIdUsuarioCreador = caja.Caja_IdUsuarioCreador,
                    CajaVigencia = true,
                    EntidadId = 4
                };
                hospital.Caja.Add(caja);
                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Caja correctamente");

                await SendMessageQueue(cajaEntities);
                Logger.Info($"La caja {cajaEntities.CajaDescripcion} se ha enviado correctamente");
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
        public async Task Actualizar()
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

                CajaEntities cajaEntities = new CajaEntities()
                {
                    CajaDescripcion = nuevaCaja.Caja_Descripcion,
                    CajaFechaCreacion = nuevaCaja.Caja_FechaCreacion,
                    CajaIdUsuarioCreador = nuevaCaja.Caja_IdUsuarioCreador,
                    CajaVigencia = true,
                    EntidadId = 4
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la Caja correctamente.");
                await SendMessageQueue(cajaEntities);
                Logger.Info($"La caja {cajaEntities.CajaDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }    

        }
        public async Task Eliminar()
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

                Caja caja = hospital.Caja.Where(
                    caj => caj.Caja_Id == id
                    ).First();
                caja.Caja_Vigencia = false;

                CajaEntities cajaEntities = new CajaEntities()
                {
                    CajaDescripcion = caja.Caja_Descripcion,
                    CajaFechaCreacion = caja.Caja_FechaCreacion,
                    CajaIdUsuarioCreador = caja.Caja_IdUsuarioCreador,
                    CajaVigencia = caja.Caja_Vigencia,
                    EntidadId = 4
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado esta Cajaf correctamente.");

                await SendMessageQueue(cajaEntities);
                Logger.Info($"La caja {cajaEntities.CajaDescripcion} se ha enviado correctamente");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }

        #region INTEGRACION
        private async Task SendMessageQueue(CajaEntities CajaEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(CajaEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
