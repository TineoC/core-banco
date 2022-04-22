using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
//??? Para Hacer Operaciones
//Funciona 
namespace Core.Controllers
{
   internal  class TipoDocumentoController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static TipoDocumentoController Instancia = null;
        public static TipoDocumentoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new TipoDocumentoController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(TipoDocumento tipoDocumento)
        {
            
            Console.WriteLine($"Descripcion del tipo de Documento: {tipoDocumento.TipoDocumento_Descripcion}");
            Console.WriteLine($"Fecha de creacion: { tipoDocumento.TipoDocumento_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: { tipoDocumento.TipoDocumento_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: { tipoDocumento.TipoDocumento_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe la descripcion de tipo de Documento: ");
                string descripcion = Console.ReadLine();

                TipoDocumento TipoDocumento = new TipoDocumento()
                {
                    TipoDocumento_Id = 0,
                    TipoDocumento_Descripcion = descripcion,
                    TipoDocumento_FechaCreacion = DateTime.Now,
                    TipoDocumento_IdUsuarioCreador = Program.loggerUserID,
                    TipoDocumento_Vigencia = true
                };

                TipoDocumentoEntities tipoDocumentoEntities = new TipoDocumentoEntities()
                {
                    TipoDocumentoId = TipoDocumento.TipoDocumento_Id,
                    TipoDocumentoDescripcion = TipoDocumento.TipoDocumento_Descripcion,
                    TipoDocumentoFechaCreacion = TipoDocumento.TipoDocumento_FechaCreacion,
                    TipoDocumentoIdUsuarioCreador = TipoDocumento.TipoDocumento_IdUsuarioCreador,
                    TipoDocumentoVigencia = true,
                    EntidadId = 17
                };

                hospital.TipoDocumento.Add(TipoDocumento);
                Logger.Info($"Se ha creado el tipo de Documento correctamente: {descripcion}");
                hospital.SaveChanges();

                await SendMessageQueue(tipoDocumentoEntities);
                Logger.Info($"El tipo de documento {tipoDocumentoEntities.TipoDocumentoDescripcion} se ha enviado correctamente");
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
            int TipoDocumento;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de Documento: ");
                TipoDocumento = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoDocumento.Any(tipoDoc => tipoDoc.TipoDocumento_Id == TipoDocumento);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Documentos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            TipoDocumento tipoDocumento = hospital.TipoDocumento
                            .Where(
                                tipoDoc => tipoDoc.TipoDocumento_Id == TipoDocumento
                            )
                            .FirstOrDefault();

            MostrarInformacion(tipoDocumento);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (TipoDocumento tipoDocumento in hospital.TipoDocumento.ToList())
            {
                Console.Clear();
                Console.WriteLine($"TipoDocumento: {index}");

                MostrarInformacion(tipoDocumento);

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int TipoDocumento;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la descripcion del tipo de Documento  a actualizar: ");
                TipoDocumento = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoDocumento.Any(tipoDoc => tipoDoc.TipoDocumento_Id == TipoDocumento);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Documentos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe la descripcion del Documento (actualizado): ");
            string descripcion = Console.ReadLine();


            TipoDocumento nuevoTipoDocumento = hospital.TipoDocumento.Where(
                    tipoDoc => tipoDoc.TipoDocumento_Id == TipoDocumento
                ).First();

            nuevoTipoDocumento.TipoDocumento_Descripcion = descripcion;

            TipoDocumentoEntities tipoDocumentoEntities = new TipoDocumentoEntities()
            {
                TipoDocumentoId = nuevoTipoDocumento.TipoDocumento_Id,
                TipoDocumentoDescripcion = nuevoTipoDocumento.TipoDocumento_Descripcion,
                TipoDocumentoFechaCreacion = nuevoTipoDocumento.TipoDocumento_FechaCreacion,
                TipoDocumentoIdUsuarioCreador = nuevoTipoDocumento.TipoDocumento_IdUsuarioCreador,
                TipoDocumentoVigencia = true,
                EntidadId = 17
            };

            Logger.Info($"El tipo de Documento con la identificacion {TipoDocumento} ha sido actualizado.");

            hospital.SaveChanges();

            await SendMessageQueue(tipoDocumentoEntities);
            Logger.Info($"El tipo de documento {tipoDocumentoEntities.TipoDocumentoDescripcion} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int TipoDocumento;

                do
                {
                    Console.Write("Escribe la identifiacion del tipo de Documento a eliminar: ");
                    TipoDocumento = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoDocumento.Any(tipoDoc => tipoDoc.TipoDocumento_Id == TipoDocumento);

                    if (!exists)
                    {
                        Console.WriteLine("No existen tipos de Documentos con esa identifiacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                TipoDocumento nuevoTipoDocumento = hospital.TipoDocumento.Where(
                        tipoDoc => tipoDoc.TipoDocumento_Id == TipoDocumento
                    ).First();

                nuevoTipoDocumento.TipoDocumento_Vigencia = false;

                TipoDocumentoEntities tipoDocumentoEntities = new TipoDocumentoEntities()
                {
                    TipoDocumentoId = nuevoTipoDocumento.TipoDocumento_Id,
                    TipoDocumentoDescripcion = nuevoTipoDocumento.TipoDocumento_Descripcion,
                    TipoDocumentoFechaCreacion = nuevoTipoDocumento.TipoDocumento_FechaCreacion,
                    TipoDocumentoIdUsuarioCreador = nuevoTipoDocumento.TipoDocumento_IdUsuarioCreador,
                    TipoDocumentoVigencia = nuevoTipoDocumento.TipoDocumento_Vigencia,
                    EntidadId = 17
                };

                hospital.SaveChanges();

                Logger.Info($"El tipo de Documento con la identifiacion {TipoDocumento} ha sido eliminado.");

                await SendMessageQueue(tipoDocumentoEntities);
                Logger.Info($"El tipo de documento {tipoDocumentoEntities.TipoDocumentoDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(TipoDocumentoEntities TipoDocumentoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(TipoDocumentoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion

    }
}
