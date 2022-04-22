using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
// Arreglar
namespace Core.Controllers
{
    internal class ReciboIngresoController

    {
        static hospitalEntities hospital = new hospitalEntities();
        public static void MostrarInformacion(ReciboIngreso reciboIngreso)
        {
            Console.WriteLine($"Identificacion del Recibo de Ingreso: {reciboIngreso.ReciboIngreso_Id}");
            Console.WriteLine($"ID del paciente: {reciboIngreso.ReciboIngreso_IdPaciente}");
            Console.WriteLine($"ID del cajero: {reciboIngreso.ReciboIngreso_IdCajero}");
            Console.WriteLine($"Monto: {reciboIngreso.ReciboIngreso_Monto}");
            Console.WriteLine($"ID del detalle de la cuenta: {reciboIngreso.ReciboIngreso_IdCuentaDetalle}");
            Console.WriteLine($"Fecha Creación: {reciboIngreso.ReciboIngreso_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {reciboIngreso.ReciboIngreso_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {reciboIngreso.ReciboIngreso_Vigencia}");
        }

        public async Task Crear()
        {
            bool exists = false;
            
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                string paciente, cajero;
                decimal monto;
                int detallecuenta;
                do
                {


                    Console.Write("Escribe el ID del paceinte: ");
                    paciente = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(recibo => recibo.Persona_Documento == paciente); 
                   var find1 = hospital.Persona.Where(recibo => recibo.Persona_TipoPersona == 1 && recibo.Persona_Documento == paciente).FirstOrDefault();
                   var find2= hospital.Persona.Where(recibo => recibo.Persona_TipoPersona == 1);
                    

                    if (!exists && find1 == null)
                    {
                        Console.WriteLine("No existe un paciente con esa identificacion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);


                do
                {


                    Console.Write("Escribe el ID del cajero: ");
                    cajero = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(recibo => recibo.Persona_Documento == cajero);
                    hospital.Persona.Where(recibo => recibo.Persona_TipoPersona == 2 && recibo.Persona_Documento ==cajero);



                    if (!exists)
                    {
                        Console.WriteLine("No existe un cajero con esa identificacion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                do
                {


                    Console.Write("Escribe el ID de los detalles de la cuenta: ");
                    detallecuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaEncabezado.Any(recibo => recibo.FacturaEncabezado_Id == detallecuenta); 

                    if (!exists)
                    {
                        Console.WriteLine("No existe un detalle de la cuenta  con esa identificacion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);



                Console.Write("Escribe el Monto: ");
                 monto = Decimal.Parse(Console.ReadLine());

                ReciboIngreso ReciboIngreso = new ReciboIngreso() {
                    ReciboIngreso_Id = 0,
                    ReciboIngreso_IdPaciente = paciente,
                    ReciboIngreso_IdCajero = cajero,
                    ReciboIngreso_Monto = monto,
                    ReciboIngreso_IdCuentaDetalle = detallecuenta,
                    ReciboIngreso_FechaCreacion = DateTime.Now,
                    ReciboIngreso_IdUsuarioCreador = Program.loggerUserID,
                    ReciboIngreso_Vigencia = true
                };

                hospital.ReciboIngreso.Add(ReciboIngreso);

                Logger.Info($"Se ha creado un Recibo de ingreso correctamente con el paciente {paciente}");

                ReciboIngresoEntities reciboIngresoEntities = new ReciboIngresoEntities() {
                    ReciboIngresoId = ReciboIngreso.ReciboIngreso_Id,
                    ReciboIngresoIdPaciente = ReciboIngreso.ReciboIngreso_IdPaciente,
                    ReciboIngresoIdCajero = ReciboIngreso.ReciboIngreso_IdCajero,
                    ReciboIngresoMonto = ReciboIngreso.ReciboIngreso_Monto,
                    ReciboIngresoIdCuentaDetalle = ReciboIngreso.ReciboIngreso_IdCuentaDetalle,
                    ReciboIngresoFechaCreacion = ReciboIngreso.ReciboIngreso_FechaCreacion,
                    ReciboIngresoIdUsuarioCreador = ReciboIngreso.ReciboIngreso_IdUsuarioCreador,
                    ReciboIngresoVigencia = true,
                    EntidadId = 16
                };

                hospital.SaveChanges();

                await SendMessageQueue(reciboIngresoEntities);
                Logger.Info($"El Recibo de ingreso del paciente {reciboIngresoEntities.ReciboIngresoIdPaciente} creada por el cajero {reciboIngresoEntities.ReciboIngresoIdCajero} se ha enviado correctamente");
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
            string paciente,cajero;
            int detallecuenta,reciboIngresoId;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion de recibo de ingreso a mostrar: ");
                reciboIngresoId = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.ReciboIngreso.Any(recibo => recibo.ReciboIngreso_Id == reciboIngresoId);

                if (!exists)
                {
                    Console.WriteLine("No existen recibo de ingresos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            
            Console.Clear();

            ReciboIngreso reciboIngreso = hospital.ReciboIngreso
                            .Where(
                                recibo => recibo.ReciboIngreso_Id == reciboIngresoId
                            )
                            .FirstOrDefault();

            MostrarInformacion(reciboIngreso);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (ReciboIngreso reciboIngreso in hospital.ReciboIngreso.ToList())
            {
                Console.Clear();
                Console.WriteLine($"ReciboIngreso: {index}");

                MostrarInformacion(reciboIngreso);

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            string cajero,paciente;
            int detallecuenta,reciboIngresoId;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del recibo de ingreso: ");
                reciboIngresoId = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.ReciboIngreso.Any(recibo => recibo.ReciboIngreso_Id == reciboIngresoId);

                if (!exists)
                {
                    Console.WriteLine("No existe recibo de ingreso un con esa identifiacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {
                Console.Write("Escribe ID del cajero a actualizar: ");
                cajero = Console.ReadLine();

                Console.Clear();

                exists = hospital.Persona.Any(recibo => recibo.Persona_Documento == cajero);
                

                if (!exists)
                {
                    Console.WriteLine("No existen un cajero con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);
            do
            {
                Console.Write("ID de los detalles de la cuenta a actualizar");
                detallecuenta = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.FacturaEncabezado.Any(recibo => recibo.FacturaEncabezado_Id == detallecuenta);

                if (!exists)
                {
                    Console.WriteLine("No existen detalles de cuenta con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);


            Console.Write("Escribe ID del paciente (actualizado): ");
             paciente = Console.ReadLine();

            Console.Write("Escribe ID del cajero (actualizado): ");
            cajero = Console.ReadLine();

            Console.Write("Escribe el ID de los detalles de la cuenta (actualizado): ");
            detallecuenta = Int32.Parse(Console.ReadLine());


            Console.Write("Escribe el monto (actualizado): ");
            decimal monto = Decimal.Parse(Console.ReadLine());



            ReciboIngreso nuevoReciboIngreso = hospital.ReciboIngreso.Where(
                    recibo => recibo.ReciboIngreso_Id == reciboIngresoId
                ).First();


            nuevoReciboIngreso.ReciboIngreso_IdPaciente = paciente;
            nuevoReciboIngreso.ReciboIngreso_IdCajero = cajero;
            nuevoReciboIngreso.ReciboIngreso_Monto = monto;
            nuevoReciboIngreso.ReciboIngreso_IdCuentaDetalle = detallecuenta;


            ReciboIngresoEntities reciboIngresoEntities = new ReciboIngresoEntities()
            {
                ReciboIngresoId = nuevoReciboIngreso.ReciboIngreso_Id,
                ReciboIngresoIdPaciente = nuevoReciboIngreso.ReciboIngreso_IdPaciente,
                ReciboIngresoIdCajero = nuevoReciboIngreso.ReciboIngreso_IdCajero,
                ReciboIngresoMonto = nuevoReciboIngreso.ReciboIngreso_Monto,
                ReciboIngresoIdCuentaDetalle = nuevoReciboIngreso.ReciboIngreso_IdCuentaDetalle,
                ReciboIngresoFechaCreacion = nuevoReciboIngreso.ReciboIngreso_FechaCreacion,
                ReciboIngresoIdUsuarioCreador = nuevoReciboIngreso.ReciboIngreso_IdUsuarioCreador,
                ReciboIngresoVigencia = true,
                EntidadId = 16
            };

            Logger.Info($"El recibo de ingreso con la identifiacion{reciboIngresoId} ha sido actualizado.");

            hospital.SaveChanges();


            await SendMessageQueue(reciboIngresoEntities);
            Logger.Info($"El Recibo de ingreso del paciente {reciboIngresoEntities.ReciboIngresoIdPaciente} creada por el cajero {reciboIngresoEntities.ReciboIngresoIdCajero} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                
                int reciboIngresoId;

                do
                {
                    Console.Write("Escribe la identificacion del recibo de ingreso: ");
                    reciboIngresoId = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.ReciboIngreso.Any(recibo => recibo.ReciboIngreso_Id == reciboIngresoId);

                    if (!exists)
                    {
                        Console.WriteLine("No existe recibo de ingreso un con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);



                ReciboIngreso nuevoReciboIngreso = hospital.ReciboIngreso.Where(
                         recibo => recibo.ReciboIngreso_Id == reciboIngresoId
                     ).First();

                nuevoReciboIngreso.ReciboIngreso_Vigencia = false;

                ReciboIngresoEntities reciboIngresoEntities = new ReciboIngresoEntities()
                {
                    ReciboIngresoId = nuevoReciboIngreso.ReciboIngreso_Id,
                    ReciboIngresoIdPaciente = nuevoReciboIngreso.ReciboIngreso_IdPaciente,
                    ReciboIngresoIdCajero = nuevoReciboIngreso.ReciboIngreso_IdCajero,
                    ReciboIngresoMonto = nuevoReciboIngreso.ReciboIngreso_Monto,
                    ReciboIngresoIdCuentaDetalle = nuevoReciboIngreso.ReciboIngreso_IdCuentaDetalle,
                    ReciboIngresoFechaCreacion = nuevoReciboIngreso.ReciboIngreso_FechaCreacion,
                    ReciboIngresoIdUsuarioCreador = nuevoReciboIngreso.ReciboIngreso_IdUsuarioCreador,
                    ReciboIngresoVigencia = nuevoReciboIngreso.ReciboIngreso_Vigencia,
                    EntidadId = 16
                };

                hospital.SaveChanges();

               Logger.Info($"El recibo de ingreso con la identificacion{reciboIngresoId}  ha sido eliminado");

                await SendMessageQueue(reciboIngresoEntities);
                Logger.Info($"El Recibo de ingreso del paciente {reciboIngresoEntities.ReciboIngresoIdPaciente} creada por el cajero {reciboIngresoEntities.ReciboIngresoIdCajero} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(ReciboIngresoEntities ReciboingresoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(ReciboingresoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
