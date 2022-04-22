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
   internal class TipoNCFController
    {
        public static TipoNCFController Instancia = null;
        public static TipoNCFController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new TipoNCFController();
            }

            return Instancia;
        }

        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(TipoNCF tipoNCF)
        {
            Console.WriteLine($"Descripcion del tipo de NCF: {tipoNCF.TipoNCF_Descripcion}");
            Console.WriteLine($"ID del usuario: { tipoNCF.TipoNCF_IdUsuario}");
            Console.WriteLine($"Fecha de Creacion: { tipoNCF.TipoNCF_FechaCreado}");
            Console.WriteLine($"Estado: { tipoNCF.TipoNCF_Estado}"); 
            Console.WriteLine($"ID de la sucural: { tipoNCF.TipoNCF_IdSucursal}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe la descripcion de tipo de NCF: ");
                string descripcion = Console.ReadLine();

                Console.Write("Escribe el ID usuario de tipo de NCF: ");
                int usuario = Int32.Parse(Console.ReadLine());

                Console.Write("Escribe el estado: ");
                bool Estado = bool.Parse(Console.ReadLine());

                Console.Write("Escribe el ID de la sucursal: ");
                int sucursal = Int32.Parse(Console.ReadLine());

                TipoNCF ncf = new TipoNCF() {
                    TipoNCF_Id =0,
                    TipoNCF_Descripcion = descripcion,
                    TipoNCF_IdUsuario = usuario,
                    TipoNCF_FechaCreado = DateTime.Now,
                    TipoNCF_Estado = Estado,
                    TipoNCF_IdSucursal = sucursal
                };

                TipoNCFEEntitites TipoNCFEEntitites = new TipoNCFEEntitites()
                {
                    TipoNcfId = ncf.TipoNCF_Id,
                    TipoNcfDescripcion = ncf.TipoNCF_Descripcion,
                    TipoNcfIdUsuario = ncf.TipoNCF_IdUsuario,
                    TipoNcfFechaCreado = ncf.TipoNCF_FechaCreado,
                    TipoNcfEstado = ncf.TipoNCF_Estado,
                    TipoNcfIdSucursal = ncf.TipoNCF_IdSucursal,
                    EntidadId = 22
                };

                hospital.TipoNCF.Add(ncf); 
                Logger.Info($"Se ha creado el tipo de NCF correctamente");
                hospital.SaveChanges();

                await SendMessageQueue(TipoNCFEEntitites);
                Logger.Info($"El tipo de NCF {TipoNCFEEntitites.TipoNcfDescripcion} se ha enviado correctamente");
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
        
            int tipoNCFid;
           
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de NCF: ");
                tipoNCFid = Int32.Parse(Console.ReadLine());


                Console.Clear();

                exists = hospital.TipoNCF.Any(tipo => tipo.TipoNCF_Id == tipoNCFid );

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de NCFs con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            TipoNCF tipoNCF = hospital.TipoNCF
                            .Where(
                                tipo => tipo.TipoNCF_Id == tipoNCFid
                            )
                            .FirstOrDefault();

            MostrarInformacion(tipoNCF);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (TipoNCF tipoNCF in hospital.TipoNCF.ToList())
            {
                Console.Clear();
                Console.WriteLine($"TipoNCF: {index}");

                MostrarInformacion(tipoNCF);

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
      
            int  tipoNCFid;
           
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de NCF  a actualizar: ");
                tipoNCFid = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoNCF.Any(tipo => tipo.TipoNCF_Id == tipoNCFid);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de NCFs con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe la descripcion de tipo de NCF (actualizada): ");
            string descripcion = Console.ReadLine();

            Console.Write("Escribe el ID usuario de tipo de NCF (actualizada): ");
            int usuario = Int32.Parse(Console.ReadLine());

            Console.Write("Escribe el estado (actualizado): ");
            bool Estado = bool.Parse(Console.ReadLine());

            Console.Write("Escribe el ID de la sucursal (actualizado): ");
            int sucursal = Int32.Parse(Console.ReadLine());
           


            TipoNCF nuevoTipoNCF = hospital.TipoNCF.Where(
                    tipo => tipo.TipoNCF_Id == tipoNCFid
                ).First();

            nuevoTipoNCF.TipoNCF_Descripcion = descripcion;
            nuevoTipoNCF.TipoNCF_IdUsuario = usuario;
            nuevoTipoNCF.TipoNCF_Estado = Estado;
            nuevoTipoNCF.TipoNCF_IdSucursal = sucursal;

            TipoNCFEEntitites TipoNCFEEntitites = new TipoNCFEEntitites()
            {
                TipoNcfId = nuevoTipoNCF.TipoNCF_Id,
                TipoNcfDescripcion = nuevoTipoNCF.TipoNCF_Descripcion,
                TipoNcfIdUsuario = nuevoTipoNCF.TipoNCF_IdUsuario,
                TipoNcfFechaCreado = nuevoTipoNCF.TipoNCF_FechaCreado,
                TipoNcfEstado = nuevoTipoNCF.TipoNCF_Estado,
                TipoNcfIdSucursal = nuevoTipoNCF.TipoNCF_IdSucursal,
                EntidadId = 22
            };

            Logger.Info($"El tipo de NCF con la identificacion {tipoNCFid} ha sido actualizado.");
            hospital.SaveChanges();

            await SendMessageQueue(TipoNCFEEntitites);
            Logger.Info($"El tipo de NCF {TipoNCFEEntitites.TipoNcfDescripcion} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int TipoNCF;

                do
                {
                    Console.Write("Escribe la identifiacion del tipo de NCF a eliminar: ");
                    TipoNCF = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoNCF.Any(tipo => tipo.TipoNCF_Id == TipoNCF);

                    if (!exists)
                    {
                        Console.WriteLine("No existen tipos de NCFs con esa identifiacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);


                TipoNCF nuevoTipoNCF = hospital.TipoNCF.Where(
                        tipo => tipo.TipoNCF_Id == TipoNCF
                    ).First();

                nuevoTipoNCF.TipoNCF_Estado = false;

                hospital.SaveChanges();
                Logger.Info($"El tipo de NCF con la identifiacion {TipoNCF} ha sido eliminado.");


                TipoNCFEEntitites TipoNCFEEntitites = new TipoNCFEEntitites()
                {
                    TipoNcfId = nuevoTipoNCF.TipoNCF_Id,
                    TipoNcfDescripcion = nuevoTipoNCF.TipoNCF_Descripcion,
                    TipoNcfIdUsuario = nuevoTipoNCF.TipoNCF_IdUsuario,
                    TipoNcfFechaCreado = nuevoTipoNCF.TipoNCF_FechaCreado,
                    TipoNcfEstado = nuevoTipoNCF.TipoNCF_Estado,
                    TipoNcfIdSucursal = nuevoTipoNCF.TipoNCF_IdSucursal,
                    EntidadId = 22
                };

                await SendMessageQueue(TipoNCFEEntitites);
                Logger.Info($"El tipo de NCF {TipoNCFEEntitites.TipoNcfDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(TipoNCFEEntitites TipoNCFEEntitites)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(TipoNCFEEntitites);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
