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
    internal class AutorizacionController
    {
        static hospitalEntities hospital = new hospitalEntities();


        public static AutorizacionController Instancia = null;
        public static AutorizacionController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new AutorizacionController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(Autorizacion autorizacion)
        {
            Console.WriteLine($"ID autorizacion: {autorizacion.Autorizacion_Id}");
            Console.WriteLine($"ID Aseguradora: {autorizacion.Autorizacion_IdAseguradora}");
            Console.WriteLine($"ID Procedimiento: {autorizacion.Autorizacion_IdProcedimiento}");
            Console.WriteLine($"Fecha Creación: {autorizacion.Autorizacion_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {autorizacion.Autorizacion_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {autorizacion.Autorizacion_Vigencia}");
            Console.WriteLine($"Precio: {autorizacion.Autorizacion_Precio}");
            Console.WriteLine($"Cobertura: {autorizacion.Autorizacion_Cobertura}");
            Console.WriteLine($"Diferencia: {autorizacion.Autorizacion_Diferencia}");
            Console.WriteLine($"Número de Autorización: {autorizacion.Autorizacion_NoAutorizacion}");
        }

        public async  Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists;
                int idAseguradora, idProcedimiento;

                do
                {
                    Console.Write("Escribe el id de la aseguradora: ");
                    idAseguradora = Int32.Parse(Console.ReadLine());
                    Console.Clear();

                    exists = hospital.Aseguradora.Any(aseg => aseg.Aseguraodra_Id == idAseguradora);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Aseguradoras con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                do
                {
                    Console.Write("Escribe el id del procedimiento: ");
                    idProcedimiento = Int32.Parse(Console.ReadLine());
                    Console.Clear();

                    exists = hospital.ProcesoMedico.Any(proc => proc.ProcesoMedico_Id == idProcedimiento);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Procesos con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                Console.Write("Escribe el precio de la autorización: ");
                Decimal precio = Decimal.Parse(Console.ReadLine());

                Decimal cobertura;

                do
                {
                    Console.Write("Escribe la cobertura de la autorización: ");
                    cobertura = Decimal.Parse(Console.ReadLine());
                    Console.Clear();

                    if (cobertura >= precio)
                    {
                        Console.WriteLine("La cobertura no peude ser mayor al precio.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (cobertura >= precio);

                Decimal diferencia = precio - cobertura;

                Console.WriteLine("Escribe el número de autorización: ");
                int numeroAutorizacion = Int32.Parse(Console.ReadLine());

                Autorizacion autorizacion = new Autorizacion()
                {
                    Autorizacion_IdAseguradora = idAseguradora,
                    Autorizacion_IdProcedimiento = idProcedimiento,
                    Autorizacion_FechaCreacion = DateTime.Now,
                    Autorizacion_IdUsuarioCreador = Program.loggerUserID,
                    Autorizacion_Vigencia = true,
                    Autorizacion_Precio = precio,
                    Autorizacion_Cobertura = cobertura,
                    Autorizacion_Diferencia = diferencia,
                    Autorizacion_NoAutorizacion = numeroAutorizacion
                };

                AutorizacionEntities autorizacionEntities = new AutorizacionEntities()
                {
                    AutorizacionIdAseguradora = autorizacion.Autorizacion_IdAseguradora,
                    AutorizacionIdProcedimiento = autorizacion.Autorizacion_IdProcedimiento,
                    AutorizacionFechaCreacion = autorizacion.Autorizacion_FechaCreacion,
                    AutorizacionIdUsuarioCreador = autorizacion.Autorizacion_IdUsuarioCreador,
                    AutorizacionVigencia = true,
                    AutorizacionPrecio = autorizacion.Autorizacion_Precio,
                    AutorizacionCobertura = autorizacion.Autorizacion_Cobertura,
                    AutorizacionDiferencia = autorizacion.Autorizacion_Diferencia,
                    AutorizacionNoAutorizacion = autorizacion.Autorizacion_NoAutorizacion,
                    EntidadId = 3
                };
                hospital.Autorizacion.Add(autorizacion);
                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Autorización correctamente");
                await SendMessageQueue(autorizacionEntities);
                Logger.Info($"La autorización de numero autorización {autorizacionEntities.AutorizacionNoAutorizacion} se ha enviado correctamente");

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
                    Console.Write("Escribe el id de la Autorización: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Autorizacion.Any(aut => aut.Autorizacion_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Autorizaciones con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                Autorizacion autorizacion = hospital.Autorizacion
                                .Where(
                                    aut => aut.Autorizacion_Id == id
                                )
                                .FirstOrDefault();

                MostrarInformacion(autorizacion);
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
                foreach (Autorizacion autorizacion in hospital.Autorizacion.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Autorización: #{index}");

                    MostrarInformacion(autorizacion);

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
                    Console.Write("Escribe el id de la Autorización: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Autorizacion.Any(aseg => aseg.Autorizacion_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Autorizaciones con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idAseguradora;

                do
                {
                    Console.Write("Escribe el id de la aseguradora: (actualizado)");
                    idAseguradora = Int32.Parse(Console.ReadLine());
                    Console.Clear();

                    exists = hospital.Aseguradora.Any(aseg => aseg.Aseguraodra_Id == idAseguradora);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Aseguradoras con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                int idProcedimiento;

                do
                {
                    Console.Write("Escribe el id del procedimiento: (actualizado) ");
                    idProcedimiento = Int32.Parse(Console.ReadLine());
                    Console.Clear();

                    exists = hospital.ProcesoMedico.Any(proc => proc.ProcesoMedico_Id == idProcedimiento);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Procesos con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                Console.Write("Escribe el precio de la autorización: (actualizado) ");
                Decimal precio = Decimal.Parse(Console.ReadLine());

                Decimal cobertura;

                do
                {
                    Console.Write("Escribe la cobertura de la autorización: (actualizado) ");
                    cobertura = Decimal.Parse(Console.ReadLine());
                    Console.Clear();

                    if (cobertura >= precio)
                    {
                        Console.WriteLine("La cobertura no peude ser mayor al precio.");
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (cobertura >= precio);

                Decimal diferencia = precio - cobertura;

                Console.WriteLine("Escribe el número de autorización: (actualizado) ");
                int numeroAutorizacion = Int32.Parse(Console.ReadLine());

                Autorizacion autorizacion = hospital.Autorizacion
                    .Where(
                        aut => aut.Autorizacion_Id == id
                    ).First();

                autorizacion.Autorizacion_IdAseguradora = idAseguradora;
                autorizacion.Autorizacion_IdProcedimiento = idProcedimiento;
                autorizacion.Autorizacion_Precio = precio;
                autorizacion.Autorizacion_Cobertura = cobertura;
                autorizacion.Autorizacion_Diferencia = diferencia;
                autorizacion.Autorizacion_NoAutorizacion = numeroAutorizacion;
                autorizacion.Autorizacion_Vigencia = true;

                AutorizacionEntities autorizacionEntities = new AutorizacionEntities()
                {
                    AutorizacionIdAseguradora = autorizacion.Autorizacion_IdAseguradora,
                    AutorizacionIdProcedimiento = autorizacion.Autorizacion_IdProcedimiento,
                    AutorizacionFechaCreacion = autorizacion.Autorizacion_FechaCreacion,
                    AutorizacionIdUsuarioCreador = autorizacion.Autorizacion_IdUsuarioCreador,
                    AutorizacionVigencia = true,
                    AutorizacionPrecio = autorizacion.Autorizacion_Precio,
                    AutorizacionCobertura = autorizacion.Autorizacion_Cobertura,
                    AutorizacionDiferencia = autorizacion.Autorizacion_Diferencia,
                    AutorizacionNoAutorizacion = autorizacion.Autorizacion_NoAutorizacion,
                    EntidadId = 3
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la Autorización correctamente.");

                await SendMessageQueue(autorizacionEntities);
                Logger.Info($"La autorización de numero autorización {autorizacionEntities.AutorizacionNoAutorizacion} se ha enviado correctamente");
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
                    Console.Write("Escribe el id de la Autorización: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Autorizacion.Any(aseg => aseg.Autorizacion_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Autorizaciones con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Autorizacion autorizacion = hospital.Autorizacion.Where(
                    aut => aut.Autorizacion_Id == id
                    ).First();
                autorizacion.Autorizacion_Vigencia = false;

                AutorizacionEntities autorizacionEntities = new AutorizacionEntities()
                {
                    AutorizacionIdAseguradora = autorizacion.Autorizacion_IdAseguradora,
                    AutorizacionIdProcedimiento = autorizacion.Autorizacion_IdProcedimiento,
                    AutorizacionFechaCreacion = autorizacion.Autorizacion_FechaCreacion,
                    AutorizacionIdUsuarioCreador = autorizacion.Autorizacion_IdUsuarioCreador,
                    AutorizacionVigencia = autorizacion.Autorizacion_Vigencia,
                    AutorizacionPrecio = autorizacion.Autorizacion_Precio,
                    AutorizacionCobertura = autorizacion.Autorizacion_Cobertura,
                    AutorizacionDiferencia = autorizacion.Autorizacion_Diferencia,
                    AutorizacionNoAutorizacion = autorizacion.Autorizacion_NoAutorizacion,
                    EntidadId = 3
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Autorización correctamente.");

                await SendMessageQueue(autorizacionEntities);
                Logger.Info($"La autorización de numero autorización {autorizacionEntities.AutorizacionNoAutorizacion} se ha enviado correctamente");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }

        #region INTEGRACION
        private async Task SendMessageQueue(AutorizacionEntities AutorizacionEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(AutorizacionEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
