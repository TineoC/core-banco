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
    internal class FacturaEncabezadoController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static FacturaEncabezadoController Instancia = null;
        public static FacturaEncabezadoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new FacturaEncabezadoController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(FacturaEncabezado facturaEncabezado)
        {
            Console.WriteLine($"ID Factura: {facturaEncabezado.FacturaEncabezado_Id}");
            Console.WriteLine($"ID NCF: {facturaEncabezado.FacturaEncabezado_IdNCF}");
            Console.WriteLine($"ID Cliente: {facturaEncabezado.FacturaEncabezado_IdCliente}");
            Console.WriteLine($"ID Cajero: {facturaEncabezado.FacturaEncabezado_IdCajero}");
            Console.WriteLine($"Total Bruto: {facturaEncabezado.FacturaEncabezado_TotalBruto}");
            Console.WriteLine($"Total Cobertura: {facturaEncabezado.FacturaEncabezado_TotalCobertura}");
            Console.WriteLine($"Total ITBIS: {facturaEncabezado.FacturaEncabezado_TotalItbis}");
            Console.WriteLine($"Total Descuento: {facturaEncabezado.FacturaEncabezado_TotalDescuento}");
            Console.WriteLine($"Total General: {facturaEncabezado.FacturaEncabezado_TotalGeneral}");
            Console.WriteLine($"Fecha Creación: {facturaEncabezado.FacturaEncabezado_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {facturaEncabezado.FacturaEncabezado_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {facturaEncabezado.FacturaEncabezado_Vigencia}");
        }

        public  async Task Crear()
        {
            bool exists = false;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                int idFactura;
                do
                {
                    Console.WriteLine("Escribe el ID de la Factura: ");
                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaEncabezado.Any(
                        factura => factura.FacturaEncabezado_Id == idFactura
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idNCF;

                do
                {
                    Console.WriteLine("Escribe el ID del NCF: ");
                    idNCF = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.NCF.Any(
                        ncf => ncf.Id == idNCF
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Plan de Tratamiento con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escriba su NCF: ");
                string NCF = Console.ReadLine();

                string documentoCliente;

                do
                {
                    Console.WriteLine("Escribe el documento del Cliente: ");
                    documentoCliente = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(
                        persona => persona.Persona_Documento == documentoCliente
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cliente con ese documento");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                string cajeroID;

                do
                {
                    Console.WriteLine("Escribe el ID del Cajero: ");
                    cajeroID = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(
                        caj => Equals(caj.Caja_Usuario_IdCaja, cajeroID)
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cajero con ese ID");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el Total Bruto: ");
                Decimal totalBruto = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Cobertura: ");
                Decimal totalCobertura = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total ITBIS: ");
                Decimal totalITBIS = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Descuento: ");
                Decimal totalDescuento = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total General: ");
                Decimal totalGeneral = Decimal.Parse(Console.ReadLine());

                FacturaEncabezado FacturaEncabezado = new FacturaEncabezado()
                {
                    FacturaEncabezado_IdNCF = idNCF,
                    FacturaEncabezado_NCF = NCF,
                    FacturaEncabezado_IdCliente = documentoCliente,
                    FacturaEncabezado_IdCajero = cajeroID,
                    FacturaEncabezado_TotalBruto = totalBruto,
                    FacturaEncabezado_TotalCobertura = totalCobertura,
                    FacturaEncabezado_TotalItbis = totalITBIS,
                    FacturaEncabezado_TotalDescuento = totalDescuento,
                    FacturaEncabezado_TotalGeneral = totalGeneral,
                    FacturaEncabezado_FechaCreacion = DateTime.Now,
                    FacturaEncabezado_IdUsuarioCreador = Program.loggerUserID,
                    FacturaEncabezado_Vigencia = true
                };


                FacturaEncabezadoEntities facturaEncabezado = new FacturaEncabezadoEntities()
                {
                    FacturaEncabezadoIdNcf = FacturaEncabezado.FacturaEncabezado_IdNCF,
                    FacturaEncabezadoNcf = FacturaEncabezado.FacturaEncabezado_NCF,
                    FacturaEncabezadoIdCliente = FacturaEncabezado.FacturaEncabezado_IdCliente,
                    FacturaEncabezadoIdCajero = FacturaEncabezado.FacturaEncabezado_IdCajero,
                    FacturaEncabezadoTotalBruto = FacturaEncabezado.FacturaEncabezado_TotalBruto,
                    FacturaEncabezadoTotalCobertura = FacturaEncabezado.FacturaEncabezado_TotalCobertura,
                    FacturaEncabezadoTotalItbis = FacturaEncabezado.FacturaEncabezado_TotalItbis,
                    FacturaEncabezadoTotalDescuento = FacturaEncabezado.FacturaEncabezado_TotalDescuento,
                    FacturaEncabezadoTotalGeneral = FacturaEncabezado.FacturaEncabezado_TotalGeneral,
                    FacturaEncabezadoFechaCreacion = FacturaEncabezado.FacturaEncabezado_FechaCreacion,
                    FacturaEncabezadoIdUsuarioCreador = FacturaEncabezado.FacturaEncabezado_IdUsuarioCreador,
                    FacturaEncabezadoVigencia = true,
                    EntidadId = 10
                };

                hospital.FacturaEncabezado.Add(FacturaEncabezado);

                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Factura correctamente");

                await SendMessageQueue(facturaEncabezado);
                Logger.Info($"Se ha enviado correctamente la factura de tipo NCF {facturaEncabezado.FacturaEncabezadoNcf} de la persona de tipo documento {facturaEncabezado.FacturaEncabezadoIdCliente}");
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
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                int idFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura: ");

                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaEncabezado.Any(fac => fac.FacturaEncabezado_Id == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningúna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                FacturaEncabezado facturaEncabezado = hospital.FacturaEncabezado
                                .Where(
                                    fac => fac.FacturaEncabezado_Id == idFactura
                                )
                                .FirstOrDefault();

                MostrarInformacion(facturaEncabezado);
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
                foreach (FacturaEncabezado factura in hospital.FacturaEncabezado.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Egreso: #{index}");

                    MostrarInformacion(factura);

                    index++;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }
        }
        public  async Task Actualizar()
        {
            bool exists = false;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                int idFactura;
                do
                {
                    Console.WriteLine("Escribe el ID de la Factura: ");
                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaEncabezado.Any(
                        factura => factura.FacturaEncabezado_Id == idFactura
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Detalle de Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdNCF;

                do
                {
                    Console.WriteLine("Escribe el ID del NCF (actualizado): ");
                    nuevoIdNCF = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.NCF.Any(
                        ncf => ncf.Id == nuevoIdNCF
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún ID del NCF");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escriba el NCF (actualizado): ");
                string NCF = Console.ReadLine();

                string nuevoDocumentoCliente;

                do
                {
                    Console.WriteLine("Escribe el documento del Cliente (actualizado): ");
                    nuevoDocumentoCliente = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(
                        persona => persona.Persona_Documento == nuevoDocumentoCliente
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cliente con ese documento");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                string nuevoCajeroID;

                do
                {
                    Console.WriteLine("Escribe el ID del Cajero (actualizado): ");
                    nuevoCajeroID = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(
                        caj => Equals(caj.Caja_Usuario_IdCaja, nuevoCajeroID)
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cajero con ese ID");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el Total Bruto (actualizado): ");
                Decimal nuevoTotalBruto = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Cobertura: ");
                Decimal nuevoTotalCobertura = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total ITBIS: ");
                Decimal nuevoTotalITBIS = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Descuento: ");
                Decimal nuevoTotalDescuento = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total General: ");
                Decimal nuevoTotalGeneral = Decimal.Parse(Console.ReadLine());

                FacturaEncabezado nuevaFactura = hospital.FacturaEncabezado
                    .Where(
                        detalle =>
                            detalle.FacturaEncabezado_Id == idFactura
                    ).First();

                nuevaFactura.FacturaEncabezado_IdNCF = nuevoIdNCF;
                nuevaFactura.FacturaEncabezado_NCF = NCF;
                nuevaFactura.FacturaEncabezado_IdCliente = nuevoDocumentoCliente;
                nuevaFactura.FacturaEncabezado_IdCajero = nuevoCajeroID;
                nuevaFactura.FacturaEncabezado_TotalBruto = nuevoTotalBruto;
                nuevaFactura.FacturaEncabezado_TotalCobertura = nuevoTotalCobertura;
                nuevaFactura.FacturaEncabezado_TotalItbis = nuevoTotalITBIS;
                nuevaFactura.FacturaEncabezado_TotalDescuento = nuevoTotalDescuento;
                nuevaFactura.FacturaEncabezado_TotalGeneral = nuevoTotalGeneral;
                nuevaFactura.FacturaEncabezado_FechaCreacion = DateTime.Now;
                nuevaFactura.FacturaEncabezado_IdUsuarioCreador = Program.loggerUserID;
                nuevaFactura.FacturaEncabezado_Vigencia = true;


                FacturaEncabezadoEntities cuentaFacturaEntities = new FacturaEncabezadoEntities()
                {
                    FacturaEncabezadoIdNcf = nuevaFactura.FacturaEncabezado_IdNCF,
                    FacturaEncabezadoNcf = nuevaFactura.FacturaEncabezado_NCF,
                    FacturaEncabezadoIdCliente = nuevaFactura.FacturaEncabezado_IdCliente,
                    FacturaEncabezadoIdCajero = nuevaFactura.FacturaEncabezado_IdCajero,
                    FacturaEncabezadoTotalBruto = nuevaFactura.FacturaEncabezado_TotalBruto,
                    FacturaEncabezadoTotalCobertura = nuevaFactura.FacturaEncabezado_TotalCobertura,
                    FacturaEncabezadoTotalItbis = nuevaFactura.FacturaEncabezado_TotalItbis,
                    FacturaEncabezadoTotalDescuento = nuevaFactura.FacturaEncabezado_TotalDescuento,
                    FacturaEncabezadoTotalGeneral = nuevaFactura.FacturaEncabezado_TotalGeneral,
                    FacturaEncabezadoFechaCreacion = nuevaFactura.FacturaEncabezado_FechaCreacion,
                    FacturaEncabezadoIdUsuarioCreador = nuevaFactura.FacturaEncabezado_IdUsuarioCreador,
                    FacturaEncabezadoVigencia = true,
                    EntidadId = 10
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la factura ID:{nuevaFactura.FacturaEncabezado_Id}.");

                await SendMessageQueue(cuentaFacturaEntities);
                Logger.Info($"Se ha enviado correctamente la factura de tipo NCF {cuentaFacturaEntities.FacturaEncabezadoNcf} de la persona de tipo documento {cuentaFacturaEntities.FacturaEncabezadoIdCliente}");
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

                int idFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura: ");

                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaEncabezado.Any(
                        factura => factura.FacturaEncabezado_Id == idFactura
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningúna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                FacturaEncabezado FacturaEncabezado = hospital.FacturaEncabezado.Where(
                        factura => factura.FacturaEncabezado_Id == idFactura
                    ).First();
                FacturaEncabezado.FacturaEncabezado_Vigencia = false;

                FacturaEncabezadoEntities cuentaFacturaEntities = new FacturaEncabezadoEntities()
                {
                    FacturaEncabezadoIdNcf = FacturaEncabezado.FacturaEncabezado_IdNCF,
                    FacturaEncabezadoNcf = FacturaEncabezado.FacturaEncabezado_NCF,
                    FacturaEncabezadoIdCliente = FacturaEncabezado.FacturaEncabezado_IdCliente,
                    FacturaEncabezadoIdCajero = FacturaEncabezado.FacturaEncabezado_IdCajero,
                    FacturaEncabezadoTotalBruto = FacturaEncabezado.FacturaEncabezado_TotalBruto,
                    FacturaEncabezadoTotalCobertura = FacturaEncabezado.FacturaEncabezado_TotalCobertura,
                    FacturaEncabezadoTotalItbis = FacturaEncabezado.FacturaEncabezado_TotalItbis,
                    FacturaEncabezadoTotalDescuento = FacturaEncabezado.FacturaEncabezado_TotalDescuento,
                    FacturaEncabezadoTotalGeneral = FacturaEncabezado.FacturaEncabezado_TotalGeneral,
                    FacturaEncabezadoFechaCreacion = FacturaEncabezado.FacturaEncabezado_FechaCreacion,
                    FacturaEncabezadoIdUsuarioCreador = FacturaEncabezado.FacturaEncabezado_IdUsuarioCreador,
                    FacturaEncabezadoVigencia = true,
                    EntidadId = 10
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Factura con el ID: {idFactura}.");

                await SendMessageQueue(cuentaFacturaEntities);
                Logger.Info($"Se ha enviado correctamente la factura de tipo NCF {cuentaFacturaEntities.FacturaEncabezadoNcf} de la persona de tipo documento {cuentaFacturaEntities.FacturaEncabezadoIdCliente}");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(FacturaEncabezadoEntities FacturaEncabezadoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(FacturaEncabezadoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
