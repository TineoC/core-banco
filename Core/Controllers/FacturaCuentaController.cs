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
    internal class FacturaCuentaController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static FacturaCuentaController Instancia = null;
        public static FacturaCuentaController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new FacturaCuentaController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(Cuenta_Factura cuentaFactura)
        {
            Console.WriteLine($"ID Factura: {cuentaFactura.Cuenta_Factura_IdFactura}");
            Console.WriteLine($"ID Cuenta: {cuentaFactura.Cuenta_Factura_IdCuenta}");
            Console.WriteLine($"Monto: {cuentaFactura.Cuenta_Factura_Monto}");
            Console.WriteLine($"Fecha Creación: {cuentaFactura.Cuenta_Factura_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {cuentaFactura.Cuenta_Factura_dUsuarioCreador}");
            Console.WriteLine($"Vigencia: {cuentaFactura.Cuenta_Factura_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.WriteLine("Escribe el ID de la Factura: ");
                int idFactura = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el ID de la Cuenta: ");
                int idCuenta = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el monto de la Factura: ");
                Decimal monto = Decimal.Parse(Console.ReadLine());

                Cuenta_Factura cuenta_Factura = new Cuenta_Factura() {
                    Cuenta_Factura_IdFactura = idFactura,
                    Cuenta_Factura_IdCuenta = idCuenta,
                    Cuenta_Factura_Monto = monto,
                    Cuenta_Factura_FechaCreacion = DateTime.Now,
                    Cuenta_Factura_dUsuarioCreador = Program.loggerUserID,
                    Cuenta_Factura_Vigencia = true
                };

                hospital.Cuenta_Factura.Add(cuenta_Factura);

                CuentaFacturaEntities cuenta_FacturaEntities = new CuentaFacturaEntities()
                {
                    CuentaFacturaIdFactura = cuenta_Factura.Cuenta_Factura_IdFactura,
                    CuentaFacturaIdCuenta = cuenta_Factura.Cuenta_Factura_IdCuenta,
                    CuentaFacturaMonto = cuenta_Factura.Cuenta_Factura_Monto,
                    CuentaFacturaFechaCreacion = cuenta_Factura.Cuenta_Factura_FechaCreacion,
                    CuentaFacturaDUsuarioCreador = cuenta_Factura.Cuenta_Factura_dUsuarioCreador,
                    CuentaFacturaVigencia = true,
                    EntidadId = 7
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Factura correctamente");

                await SendMessageQueue(cuenta_FacturaEntities);
                Logger.Info($"Se ha enviado correctamente la cuenta factura Id Factura {cuenta_FacturaEntities.CuentaFacturaIdFactura} , Id Cuenta {cuenta_FacturaEntities.CuentaFacturaIdCuenta}");
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

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == idCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el monto de la Factura");
                Decimal monto = Decimal.Parse(Console.ReadLine());

                Console.Clear();

                Cuenta_Factura cuenta_Factura = hospital.Cuenta_Factura
                                .Where(
                                    cuent_fac => 
                                        cuent_fac.Cuenta_Factura_IdFactura == idFactura
                                        &&
                                        cuent_fac.Cuenta_Factura_IdCuenta == idCuenta
                                )
                                .FirstOrDefault();

                MostrarInformacion(cuenta_Factura);
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
                foreach (Cuenta_Factura cuenta_Factura in hospital.Cuenta_Factura.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Facturas_Cuenta: #{index}");

                    MostrarInformacion(cuenta_Factura);

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
                int idFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura: ");

                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == idCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura (actualizado): ");

                    nuevoIdFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == nuevoIdFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta (actualizado): ");

                    nuevoIdCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == nuevoIdCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Cuenta_Factura cuenta_factura = hospital.Cuenta_Factura
                    .Where(
                    cuent_fac =>
                        cuent_fac.Cuenta_Factura_IdFactura == idFactura
                        &&
                        cuent_fac.Cuenta_Factura_IdCuenta == idCuenta
                    ).First();

                cuenta_factura.Cuenta_Factura_IdFactura = nuevoIdFactura;
                cuenta_factura.Cuenta_Factura_IdCuenta = nuevoIdCuenta;
                cuenta_factura.Cuenta_Factura_Vigencia = true;

                CuentaFacturaEntities cuenta_FacturaEntities = new CuentaFacturaEntities()
                {
                    CuentaFacturaIdFactura = cuenta_factura.Cuenta_Factura_IdFactura,
                    CuentaFacturaIdCuenta = cuenta_factura.Cuenta_Factura_IdCuenta,
                    CuentaFacturaMonto = cuenta_factura.Cuenta_Factura_Monto,
                    CuentaFacturaFechaCreacion = cuenta_factura.Cuenta_Factura_FechaCreacion,
                    CuentaFacturaDUsuarioCreador = cuenta_factura.Cuenta_Factura_dUsuarioCreador,
                    CuentaFacturaVigencia = true,
                    EntidadId = 7
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la Factura de la Cuenta correctamente.");

                await SendMessageQueue(cuenta_FacturaEntities);
                Logger.Info($"Se ha enviado correctamente la cuenta factura Id Factura {cuenta_FacturaEntities.CuentaFacturaIdFactura} , Id Cuenta {cuenta_FacturaEntities.CuentaFacturaIdCuenta}");
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

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == idCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Cuenta_Factura cuenta_Factura = hospital.Cuenta_Factura.Where(
                     cuent_fac =>
                             cuent_fac.Cuenta_Factura_IdFactura == idFactura
                             &&
                             cuent_fac.Cuenta_Factura_IdCuenta == idCuenta
                     ).First();

                cuenta_Factura.Cuenta_Factura_Vigencia = false;

                CuentaFacturaEntities cuenta_FacturaEntities = new CuentaFacturaEntities()
                {
                    CuentaFacturaIdFactura = cuenta_Factura.Cuenta_Factura_IdFactura,
                    CuentaFacturaIdCuenta = cuenta_Factura.Cuenta_Factura_IdCuenta,
                    CuentaFacturaMonto = cuenta_Factura.Cuenta_Factura_Monto,
                    CuentaFacturaFechaCreacion = cuenta_Factura.Cuenta_Factura_FechaCreacion,
                    CuentaFacturaDUsuarioCreador = cuenta_Factura.Cuenta_Factura_dUsuarioCreador,
                    CuentaFacturaVigencia = cuenta_Factura.Cuenta_Factura_Vigencia,
                    EntidadId = 7
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado esta Factura con el ID: {idFactura} y la Cuenta ID: {idCuenta} correctamente.");

                await SendMessageQueue(cuenta_FacturaEntities);
                Logger.Info($"Se ha enviado correctamente la cuenta factura Id Factura {cuenta_FacturaEntities.CuentaFacturaIdFactura} , Id Cuenta {cuenta_FacturaEntities.CuentaFacturaIdCuenta}");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }

        #region INTEGRACION
        private async Task SendMessageQueue(CuentaFacturaEntities CuentaFacturaEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(CuentaFacturaEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
