using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;

namespace Core.Controllers
{
    internal class AseguradoraController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static AseguradoraController Instancia = null;
        public static AseguradoraController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new AseguradoraController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(Aseguradora aseguradora)
        {
            Console.WriteLine($"ID: {aseguradora.Aseguraodra_Id}");
            Console.WriteLine($"Descripción: {aseguradora.Aseguradora_Descripcion}");
            Console.WriteLine($"Fecha Creación: {aseguradora.Aseguradora_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {aseguradora.Aseguradora_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {aseguradora.Aseguradora_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe el nombre de la aseguradora: ");
                string nombre = Console.ReadLine();

                Aseguradora aseguradora = new Aseguradora() {
                    Aseguradora_Descripcion = nombre,
                    Aseguradora_FechaCreacion = DateTime.Now,
                    Aseguradora_IdUsuarioCreador = Program.loggerUserID,
                    Aseguradora_Vigencia = true,
                };

                AseguradoraEntities aseguradoraEntity = new AseguradoraEntities()
                {
                    AseguradoraDescripcion = aseguradora.Aseguradora_Descripcion,
                    AseguradoraFechaCreacion = aseguradora.Aseguradora_FechaCreacion,
                    AseguradoraIdUsuarioCreador = aseguradora.Aseguradora_IdUsuarioCreador,
                    AseguradoraVigencia = true,
                    EntidadId = 2

                };
                hospital.Aseguradora.Add(aseguradora);

                Logger.Info($"Se ha creado la Aseguradora correctamente");

                hospital.SaveChanges();

                await SendMessageQueue(aseguradoraEntity);
                Logger.Info($"La aseguradora {aseguradoraEntity.AseguradoraDescripcion} se ha enviado correctamente");
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

                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public async Task Actualizar()
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

            Console.Write("Escribe el nombre de la aseguradora: (actualizado) ");

            string nombre = Console.ReadLine();

            Aseguradora aseguradora = hospital.Aseguradora
                .Where(
                    aseg => aseg.Aseguraodra_Id == id
                ).First();


            aseguradora.Aseguradora_Descripcion = nombre;
            aseguradora.Aseguradora_Vigencia = true;

            AseguradoraEntities aseguradoraEntity = new AseguradoraEntities()
            {
                AseguradoraDescripcion = aseguradora.Aseguradora_Descripcion,
                AseguradoraFechaCreacion = aseguradora.Aseguradora_FechaCreacion,
                AseguradoraIdUsuarioCreador = aseguradora.Aseguradora_IdUsuarioCreador,
                AseguradoraVigencia = true,
                EntidadId = 2

            };

            hospital.SaveChanges();

            Logger.Info($"Se ha actualizado la Aseguradora correctamente.");
            await SendMessageQueue(aseguradoraEntity);
            Logger.Info($"La aseguradora {aseguradoraEntity.AseguradoraDescripcion} se ha enviado correctamente");

        }
        public async  Task Eliminar()
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

                Aseguradora aseguradora = hospital.Aseguradora.Where(
                    aseg => aseg.Aseguraodra_Id == id
                    ).First();

                aseguradora.Aseguradora_Vigencia = false;

                AseguradoraEntities aseguradoraEntity = new AseguradoraEntities()
                {
                    AseguradoraDescripcion = aseguradora.Aseguradora_Descripcion,
                    AseguradoraFechaCreacion = aseguradora.Aseguradora_FechaCreacion,
                    AseguradoraIdUsuarioCreador = aseguradora.Aseguradora_IdUsuarioCreador,
                    AseguradoraVigencia = aseguradora.Aseguradora_Vigencia,
                    EntidadId = 2

                };

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Aseguradora ID: {id} correctamente.");

                await SendMessageQueue(aseguradoraEntity);
                Logger.Info($"La aseguradora {aseguradoraEntity.AseguradoraDescripcion} se ha enviado correctamente");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }

        #region INTEGRACION
        private async Task SendMessageQueue(AseguradoraEntities AseguradoraEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(AseguradoraEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
