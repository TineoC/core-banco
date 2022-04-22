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
    internal class CuentasController
    {
        static hospitalEntities hospital = new hospitalEntities();


        public static CuentasController Instancia = null;
        public static CuentasController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new CuentasController();
            }

            return Instancia;
        }


        public static void MostrarInformacion(Cuentas cuenta)
        {
            Console.WriteLine($"ID Cuenta: {cuenta.Cuenta_Id}");
            Console.WriteLine($"Documento de la Persona: {cuenta.Cuenta_IdPersona}");
            Console.WriteLine($"Balance: {cuenta.Cuenta_Balance}");
            Console.WriteLine($"Fecha Creación: {cuenta.Cuenta_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {cuenta.Cuenta_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {cuenta.Cuenta_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists;
                string documento;

                do
                {
                    Console.Clear();

                    Console.Write("Escribe el documento de la Persona: ");

                    Console.WriteLine("Escribe el documento de la Persona");
                    documento = Console.ReadLine();

                    exists = hospital.Persona.Any(
                        person => person.Persona_Documento == documento);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese Documento");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                Cuentas cuentas = new Cuentas()
                {
                    Cuenta_IdPersona = documento,
                    Cuenta_Balance = 0,
                    Cuenta_FechaCreacion = DateTime.Now,
                    Cuenta_IdUsuarioCreador = Program.loggerUserID,
                    Cuenta_Vigencia = true
                };
                hospital.Cuentas.Add(cuentas);

                CuentaEntities cuentaEntities = new CuentaEntities()
                {
                    CuentaIdPersona = cuentas.Cuenta_IdPersona,
                    CuentaBalance = cuentas.Cuenta_Balance,
                    CuentaFechaCreacion = cuentas.Cuenta_FechaCreacion,
                    CuentaIdUsuarioCreador = cuentas.Cuenta_IdUsuarioCreador,
                    CuentaVigencia = true,
                    EntidadId = 6
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Cuenta correctamente");

                await SendMessageQueue(cuentaEntities);
                Logger.Info($"La cuenta para la persona {cuentaEntities.CuentaIdPersona} se ha enviado correctamente");
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
                int idCuenta;

                do
                {
                    Console.Clear();

                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == idCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                Cuentas cuenta = hospital.Cuentas
                                .Where(
                                    cuentas => cuentas.Cuenta_Id == idCuenta
                                )
                                .FirstOrDefault();

                MostrarInformacion(cuenta);
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
                foreach (Cuentas cuenta in hospital.Cuentas.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Cuenta: #{index}");

                    MostrarInformacion(cuenta);

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
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(
                        cuenta => cuenta.Cuenta_Id == idCuenta
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                decimal nuevoBalance;

                do
                {
                    Console.Write("Escribe el Balance de la Cuenta: ");
                    nuevoBalance = decimal.Parse(Console.ReadLine());

                    Logger.Error($"Intento de Nuevo Balance: {nuevoBalance}");

                    if (nuevoBalance <= 0)
                    {
                        Logger.Error("El balance no puede ser menor o igual a cero");
                        Console.WriteLine("El balance no puede ser menor o igual a cero");
                    }

                } while (!(nuevoBalance <= 0));

                Cuentas nuevaCuenta = hospital.Cuentas
                    .Where(
                        cuenta =>
                            cuenta.Cuenta_Id == idCuenta
                    ).First();

                nuevaCuenta.Cuenta_Balance = nuevoBalance;
                nuevaCuenta.Cuenta_Vigencia = true;

                CuentaEntities cuentaEntities = new CuentaEntities()
                {
                    CuentaIdPersona = nuevaCuenta.Cuenta_IdPersona,
                    CuentaBalance = nuevaCuenta.Cuenta_Balance,
                    CuentaFechaCreacion = nuevaCuenta.Cuenta_FechaCreacion,
                    CuentaIdUsuarioCreador = nuevaCuenta.Cuenta_IdUsuarioCreador,
                    CuentaVigencia = true,
                    EntidadId = 6
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la Cuenta ID: {nuevaCuenta.Cuenta_Id}.");
                Console.WriteLine($"Se ha actualizado la Cuenta ID: {nuevaCuenta.Cuenta_Id}.");

                await SendMessageQueue(cuentaEntities);
                Logger.Info($"La cuenta para la persona {cuentaEntities.CuentaIdPersona} se ha enviado correctamente");

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

                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(
                        cuenta => cuenta.Cuenta_Id == idCuenta
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Cuentas cuenta1 = hospital.Cuentas.Where(
                    cuent =>
                            cuent.Cuenta_Id == idCuenta
                    ).First();
                cuenta1.Cuenta_Vigencia = false;

                CuentaEntities cuentaEntities = new CuentaEntities()
                {
                    CuentaIdPersona = cuenta1.Cuenta_IdPersona,
                    CuentaBalance = cuenta1.Cuenta_Balance,
                    CuentaFechaCreacion = cuenta1.Cuenta_FechaCreacion,
                    CuentaIdUsuarioCreador = cuenta1.Cuenta_IdUsuarioCreador,
                    CuentaVigencia = cuenta1.Cuenta_Vigencia,
                    EntidadId = 6
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Cuenta con el ID: {idCuenta}.");
                await SendMessageQueue(cuentaEntities);
                Logger.Info($"La cuenta para la persona {cuentaEntities.CuentaIdPersona} se ha enviado correctamente");

            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }

        #region INTEGRACION
        private async Task SendMessageQueue(CuentaEntities CuentaEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(CuentaEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }

}
