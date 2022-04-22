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
    internal class TipoProcesoController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static TipoProcesoController Instancia = null;
        public static TipoProcesoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new TipoProcesoController();
            }

            return Instancia;
        }


        public static void MostrarInformacion(TipoProceso tipoproceso)
        {
            Console.WriteLine($"Identificacion del tipo de proceso: {tipoproceso.TipoProceso_Id}");
            Console.WriteLine($"Descripcion del tipo de proceso: {tipoproceso.TipoProceso_Descripcion}");
            Console.WriteLine($"Fecha de creacion: { tipoproceso.TipoProceso_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: { tipoproceso.TipoProceso_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: { tipoproceso.TipoProceso_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists = true;
                string descripcion;
                do
                {


                    Console.Write("Escribe la descripcion de tipo de proceso: ");
                    descripcion = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.TipoProceso.Any(procMed => procMed.TipoProceso_Descripcion == descripcion);

                    if (exists)
                    {
                        Console.WriteLine("Existe un tipo de proceso con esa descripcion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);

                TipoProceso TipoProceso = new TipoProceso()
                {
                    TipoProceso_Descripcion = descripcion,
                    TipoProceso_FechaCreacion = DateTime.Now,
                    TipoProceso_IdUsuarioCreador = Program.loggerUserID,
                    TipoProceso_Vigencia = true
                };

                TipoProcesoEntities TipoProcesoEntities = new TipoProcesoEntities()
                {
                    TipoProcesoId = TipoProceso.TipoProceso_Id,
                    TipoProcesoDescripcion = TipoProceso.TipoProceso_Descripcion,
                    TipoProcesoFechaCreacion = TipoProceso.TipoProceso_FechaCreacion,
                    TipoProcesoIdUsuarioCreador = TipoProceso.TipoProceso_IdUsuarioCreador,
                    TipoProcesoVigencia = true,
                    EntidadId = 20
                };

                hospital.TipoProceso.Add(TipoProceso);
                Logger.Info($"Se ha creado el tipo de proceso correctamente: {descripcion}");
                hospital.SaveChanges();

                await SendMessageQueue(TipoProcesoEntities);
                Logger.Info($"El tipo de proceso {TipoProcesoEntities.TipoProcesoDescripcion} se ha enviado correctamente");
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

                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int TipoProceso;
            string descripcion;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el ID del tipo de proceso a actualizar: ");
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

            do
            {
                exists = true;
                Console.Write("Escribe la descripcion del tipo de proceso (actualizado): ");
                descripcion = Console.ReadLine();

                Console.Clear();

                exists = hospital.TipoProceso.Any(tipopro => tipopro.TipoProceso_Descripcion == descripcion);

                if (exists)
                {
                    Console.WriteLine("Existen tipos de procesos con esa descripcion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);

            TipoProceso nuevoTipoProceso = hospital.TipoProceso.Where(
                    tipopro => tipopro.TipoProceso_Id == TipoProceso
                ).First();

            nuevoTipoProceso.TipoProceso_Descripcion = descripcion;

            Logger.Info($"El tipo de proceso con la identificacion {TipoProceso} ha sido actualizado.");

            Console.WriteLine($"El tipo de proceso con la identificacion {TipoProceso} ha sido actualizado.");

            hospital.SaveChanges();

            TipoProcesoEntities TipoProcesoEntities = new TipoProcesoEntities()
            {
                TipoProcesoId = nuevoTipoProceso.TipoProceso_Id,
                TipoProcesoDescripcion = nuevoTipoProceso.TipoProceso_Descripcion,
                TipoProcesoFechaCreacion = nuevoTipoProceso.TipoProceso_FechaCreacion,
                TipoProcesoIdUsuarioCreador = nuevoTipoProceso.TipoProceso_IdUsuarioCreador,
                TipoProcesoVigencia = true,
                EntidadId = 20
            };

            await SendMessageQueue(TipoProcesoEntities);
            Logger.Info($"El tipo de proceso {TipoProcesoEntities.TipoProcesoDescripcion} se ha enviado correctamente");
        }
        public async Task Eliminar()
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

                TipoProceso TipoProcesoEliminar = hospital.TipoProceso.Where(
                        tipopro => tipopro.TipoProceso_Id == TipoProceso
                    ).First();

                TipoProcesoEliminar.TipoProceso_Vigencia = false;

                TipoProcesoEntities TipoProcesoEntities = new TipoProcesoEntities()
                {
                    TipoProcesoId = TipoProcesoEliminar.TipoProceso_Id,
                    TipoProcesoDescripcion = TipoProcesoEliminar.TipoProceso_Descripcion,
                    TipoProcesoFechaCreacion = TipoProcesoEliminar.TipoProceso_FechaCreacion,
                    TipoProcesoIdUsuarioCreador = TipoProcesoEliminar.TipoProceso_IdUsuarioCreador,
                    TipoProcesoVigencia = TipoProcesoEliminar.TipoProceso_Vigencia,
                    EntidadId = 20
                };

                hospital.SaveChanges();

                Logger.Info($"El tipo de proceso con la identifiacion {TipoProceso} ha sido eliminado.");

                Console.WriteLine($"El tipo de proceso con la identifiacion {TipoProceso} ha sido eliminado.");

                await SendMessageQueue(TipoProcesoEntities);
                Logger.Info($"El tipo de proceso {TipoProcesoEntities.TipoProcesoDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(TipoProcesoEntities TipoProcesoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(TipoProcesoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }

}
