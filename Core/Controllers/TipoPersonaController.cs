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
    internal class TipoPersonaController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static TipoPersonaController Instancia = null;
        public static TipoPersonaController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new TipoPersonaController();
            }

            return Instancia;
        }


        public static void MostrarInformacion(TipoPersona tipoPersona)
        {
            Console.WriteLine($"Identificacion del tipo de Persona: {tipoPersona.TipoPersona_Id}");
            Console.WriteLine($"Descripcion del tipo de Persona: {tipoPersona.TipoPersona_Descripcion}");
            Console.WriteLine($"Fecha de creacion: { tipoPersona.TipoPersona_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: { tipoPersona.TipoPersona_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: { tipoPersona.TipoPersona_Vigencia}");
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

                TipoPersona TipoPersona = new TipoPersona()
                {
                    TipoPersona_Id = 0,
                    TipoPersona_Descripcion = descripcion,
                    TipoPersona_FechaCreacion = DateTime.Now,
                    TipoPersona_IdUsuarioCreador = Program.loggerUserID,
                    TipoPersona_Vigencia = true,

                };

                    Console.Clear();

                    exists = hospital.TipoPersona.Any(tipopers => tipopers.TipoPersona_Descripcion == descripcion);

                    if (exists)
                    {
                        Console.WriteLine("Existe un tipo de persona con esa descripcion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);

                hospital.TipoPersona.Add(new TipoPersona()
                {
                    TipoPersonaId = TipoPersona.TipoPersona_Id,
                    TipoPersonaDescripcion = TipoPersona.TipoPersona_Descripcion,
                    TipoPersonaFechaCreacion = TipoPersona.TipoPersona_FechaCreacion,
                    TipoPersonaIdUsuarioCreador = TipoPersona.TipoPersona_IdUsuarioCreador,
                    TipoPersonaVigencia = true,
                    EntidadId = 19
                };

                hospital.TipoPersona.Add(TipoPersona);
                Logger.Info($"Se ha creado el tipo de Persona correctamente: {descripcion}");
                hospital.SaveChanges();

                await SendMessageQueue(TipoPersonaEntities);
                Logger.Info($"El tipo de persona {TipoPersonaEntities.TipoPersonaDescripcion} se ha enviado correctamente");
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
            int TipoPersona;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de Persona: ");
                TipoPersona = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoPersona.Any(tipoPers => tipoPers.TipoPersona_Id == TipoPersona);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Personas con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            TipoPersona tipoPersona = hospital.TipoPersona
                            .Where(
                                tipoPers => tipoPers.TipoPersona_Id == TipoPersona
                            )
                            .FirstOrDefault();

            MostrarInformacion(tipoPersona);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (TipoPersona tipoPersona in hospital.TipoPersona.ToList())
            {
                Console.Clear();
                Console.WriteLine($"TipoPersona: {index}");

                MostrarInformacion(tipoPersona);
                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int TipoPersona;
            string descripcion;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identifiacion del tipo de Persona  a actualizar: ");
                TipoPersona = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoPersona.Any(tipoPers => tipoPers.TipoPersona_Id == TipoPersona);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Personas con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {
                exists = true;
                Console.Write("Escribe la descripcion del tipo de persona (actualizado): ");
                descripcion = Console.ReadLine();

                Console.Clear();

                exists = hospital.TipoPersona.Any(tipopers => tipopers.TipoPersona_Descripcion == descripcion);

                if (exists)
                {
                    Console.WriteLine("Existen tipos de personas con esa descripcion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);


            TipoPersona nuevoTipoPersona = hospital.TipoPersona.Where(
                    tipoPers => tipoPers.TipoPersona_Id == TipoPersona
                ).First();

            nuevoTipoPersona.TipoPersona_Descripcion = descripcion;

            TipoPersonaEntities TipoPersonaEntities = new TipoPersonaEntities()
            {
                TipoPersonaId = nuevoTipoPersona.TipoPersona_Id,
                TipoPersonaDescripcion = nuevoTipoPersona.TipoPersona_Descripcion,
                TipoPersonaFechaCreacion = nuevoTipoPersona.TipoPersona_FechaCreacion,
                TipoPersonaIdUsuarioCreador = nuevoTipoPersona.TipoPersona_IdUsuarioCreador,
                TipoPersonaVigencia = true,
                EntidadId = 19
            };


            Logger.Info($"El tipo de Persona con la identificacion {TipoPersona} ha sido actualizado.");
            hospital.SaveChanges();

            await SendMessageQueue(TipoPersonaEntities);
            Logger.Info($"El tipo de persona {TipoPersonaEntities.TipoPersonaDescripcion} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int TipoPersona;

                do
                {
                    Console.Write("Escribe la identificacion del tipo de Persona a eliminar: ");
                    TipoPersona = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoPersona.Any(tipoPers => tipoPers.TipoPersona_Id == TipoPersona);

                    if (!exists)
                    {
                        Console.WriteLine("No existen tipos de Personas con esa identifiacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                TipoPersona nuevoTipoPersona = hospital.TipoPersona.Where(
                        tipoPers => tipoPers.TipoPersona_Id == TipoPersona
                    ).First();

                nuevoTipoPersona.TipoPersona_Vigencia = false;
                TipoPersonaEntities TipoPersonaEntities = new TipoPersonaEntities()
                {
                    TipoPersonaId = nuevoTipoPersona.TipoPersona_Id,
                    TipoPersonaDescripcion = nuevoTipoPersona.TipoPersona_Descripcion,
                    TipoPersonaFechaCreacion = nuevoTipoPersona.TipoPersona_FechaCreacion,
                    TipoPersonaIdUsuarioCreador = nuevoTipoPersona.TipoPersona_IdUsuarioCreador,
                    TipoPersonaVigencia = nuevoTipoPersona.TipoPersona_Vigencia,
                    EntidadId = 19
                };

                hospital.SaveChanges();
                Logger.Info($"El tipo de Persona con la identifiacion {TipoPersona} ha sido eliminado.");

                await SendMessageQueue(TipoPersonaEntities);
                Logger.Info($"El tipo de persona {TipoPersonaEntities.TipoPersonaDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }


        #region INTEGRACION
        private async Task SendMessageQueue(TipoPersonaEntities TipoPersonaEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(TipoPersonaEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
