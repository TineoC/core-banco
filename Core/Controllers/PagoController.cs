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
    internal class PagoController
    {
        public static PagoController Instancia = null;
        public static PagoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new PagoController();
            }

            return Instancia;
        }

        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(Pago pago)
        {
            Console.WriteLine($"Identificacion de pago: {pago.Pago_Id}");
            Console.WriteLine($"ID de la persona: {pago.Pago_IdPersona}");
            Console.WriteLine($"ID de la cuenta {pago.Pago_IdCuenta}");
            Console.WriteLine($"Referencias: {pago.Pago_Referencia}");
            Console.WriteLine($"Monto: {pago.Pago_Monto}");
            Console.WriteLine($"Tipo de Pago: {pago.Pago_TipoPago}");  
            Console.WriteLine($"Id de Caja: {pago.Pago_IdCaja}");
            Console.WriteLine($"Fecha Creación: {pago.Pago_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {pago.Pago_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {pago.Pago_Vigencia}"); 
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists = false;
                
                int tipopago,cuenta;
                string persona;

                do
                {

                    Console.Write("Escribe el Documento de la persona: ");
                     persona = Console.ReadLine();
                  

                    Console.Clear();

                    exists = hospital.Persona.Any(pg => pg.Persona_Documento == persona);

                    if (!exists)
                    {
                        Console.WriteLine("No existen personas con ese documento");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                do
                {
                    Console.Write("Escribe el ID de la cuenta: ");
                    cuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(pg => pg.Cuenta_Id == cuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe una cuenta con esa identificacion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                do
                {

                    Console.Write("Escribe el tipo de pago: ");
                     tipopago = Int32.Parse(Console.ReadLine());


                    Console.Clear();

                    exists = hospital.TipoPago.Any(pg => pg.TipoPago_Id == tipopago);

                    if (!exists)
                    {
                        Console.WriteLine("No existen personas con ese documento");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);
                
                Console.Write("Escribe la referencia: ");
                string referencia = Console.ReadLine();
                Console.Write("Escribe el ID de la Caja: ");
                int caja = Int32.Parse(Console.ReadLine());
                Console.Write("Escribe el monto: ");
                float monto = float.Parse(Console.ReadLine());

                Pago pago = new Pago()
                {
                    Pago_IdPersona = persona,
                    Pago_IdCuenta = cuenta,
                    Pago_Referencia = referencia,
                    Pago_Monto = monto,
                    Pago_TipoPago = tipopago,
                    Pago_FechaCreacion = DateTime.Now,
                    Pago_Vigencia = true,
                    Pago_IdUsuarioCreador = Program.loggerUserID,
                    Pago_IdCaja = caja
                    
                };
                hospital.Pago.Add(pago);

                PagoEntities pagoEntities = new PagoEntities() {
                    PagoId = pago.Pago_Id,
                    PagoIdPersona = pago.Pago_IdPersona,
                    PagoIdCuenta = pago.Pago_IdCuenta,
                    PagoReferencia = pago.Pago_Referencia,
                    PagoMonto = Convert.ToDecimal(pago.Pago_Monto),
                    PagoTipoPago = pago.Pago_TipoPago,
                    PagoFechaCreacion = pago.Pago_FechaCreacion,
                    PagoVigencia = true,
                    PagoIdUsuarioCreador = pago.Pago_IdUsuarioCreador,
                    CajaId = pago.Pago_IdCaja,
                    EntidadId = 11
                };


                Logger.Info($"Se ha creado la Pago correctamente");

                hospital.SaveChanges();

                await SendMessageQueue(pagoEntities);
                Logger.Info($"El pago {pagoEntities.PagoReferencia} se ha enviado correctamente");
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

            int pagoId;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identifiacion del pago a mostrar: ");
                pagoId = Int32.Parse(Console.ReadLine());

                


                Console.Clear();

                exists = hospital.Pago.Any(pg => pg.Pago_Id == pagoId);


                if (!exists)
                {
                    Console.WriteLine("No existen Pagos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            

            Console.Clear();

            Pago pago = hospital.Pago
                            .Where(
                                pg => pg.Pago_Id == pagoId
                            )
                            .FirstOrDefault();

            MostrarInformacion(pago);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (Pago pago in hospital.Pago.ToList())
            {
                Console.Clear();
                Console.WriteLine($"Pago: {index}");

                MostrarInformacion(pago);

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int cuenta,tipopago,idPago;
            string persona;
            var Logger = NLog.LogManager.GetCurrentClassLogger();


           

            do
            {
                Console.Write("Escribe la identificacion del pago actualizar: ");
                idPago = Int32.Parse(Console.ReadLine());




                Console.Clear();

                exists = hospital.Pago.Any(pg => pg.Pago_Id == idPago);


                if (!exists)
                {
                    Console.WriteLine("No existen Pagos con esa identifiacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {


                Console.Write("Escribe el ID de persona (actualizado): ");
                persona = Console.ReadLine();


                Console.Clear();

                exists = hospital.Persona.Any(pg => pg.Persona_Documento == persona);
                hospital.Persona.Where(recibo => recibo.Persona_TipoPersona == 1 && recibo.Persona_Documento == persona);


                if (!exists)
                {
                    Console.WriteLine("No existen Pagos con a esa persona");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);


            do
            {


                Console.Write("Escribe el ID de la cuenta (actualizado): ");
                cuenta = Int32.Parse(Console.ReadLine());


                Console.Clear();

                exists = hospital.Cuentas.Any(pg => pg.Cuenta_Id == cuenta);


                if (!exists)
                {
                    Console.WriteLine("No existen Pagos con a esa persona");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);


            do
            {
                Console.Write("Escribe el  tipo de pago  (actualizado): ");
                tipopago = Int32.Parse(Console.ReadLine());




                Console.Clear();

                exists = hospital.TipoPago.Any(pg => pg.TipoPago_Id == tipopago);


                if (!exists)
                {
                    Console.WriteLine("No existen Pagos con a ese tipo de pago");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe la ID de la caja (actualizada: ");
            int caja = Int32.Parse(Console.ReadLine());

            Console.Write("Escribe la referencia (actualizado: ");
            string referencia = Console.ReadLine();

            Console.Write("Escribe tu nuevo monto: ");
            float monto= float.Parse(Console.ReadLine());

            

            Pago nuevoPago = hospital.Pago.Where(
                    pers => pers.Pago_Id== idPago
                ).First();



            nuevoPago.Pago_IdPersona = persona;
            nuevoPago.Pago_IdCuenta = cuenta;
            nuevoPago.Pago_Referencia = referencia;
            nuevoPago.Pago_Monto = monto;
            nuevoPago.Pago_TipoPago = tipopago;
            nuevoPago.Pago_IdUsuarioCreador = Program.loggerUserID;
            nuevoPago.Pago_IdCaja = caja;

            PagoEntities pagoEntities = new PagoEntities()
            {
                PagoId = nuevoPago.Pago_Id,
                PagoIdPersona = nuevoPago.Pago_IdPersona,
                PagoIdCuenta = nuevoPago.Pago_IdCuenta,
                PagoReferencia = nuevoPago.Pago_Referencia,
                PagoMonto = Convert.ToDecimal(nuevoPago.Pago_Monto),
                PagoTipoPago = nuevoPago.Pago_TipoPago,
                PagoFechaCreacion = nuevoPago.Pago_FechaCreacion,
                PagoVigencia = true,
                PagoIdUsuarioCreador = nuevoPago.Pago_IdUsuarioCreador,
                CajaId = nuevoPago.Pago_IdCaja,
                EntidadId = 11
            };

            Logger.Info($"El Pago con el ID: {idPago} ha sido actualizado.");

            hospital.SaveChanges();

            await SendMessageQueue(pagoEntities);
            Logger.Info($"El pago {pagoEntities.PagoReferencia} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int cuenta, tipopago, idPago;
                string persona;

                do
                {
                    Console.Write("Escribe el ID del pago a actualizar: ");
                    idPago = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Pago.Any(pg => pg.Pago_Id == idPago);


                    if (!exists)
                    {
                        Console.WriteLine("No existen Pagos con a esa cuenta");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);



                Pago pago = hospital.Pago.Where(
                       pg => pg.Pago_Id == idPago
                   ).First();

                pago.Pago_Vigencia = false;

                PagoEntities pagoEntities = new PagoEntities()
                {
                    PagoId = pago.Pago_Id,
                    PagoIdPersona = pago.Pago_IdPersona,
                    PagoIdCuenta = pago.Pago_IdCuenta,
                    PagoReferencia = pago.Pago_Referencia,
                    PagoMonto = Convert.ToDecimal(pago.Pago_Monto),
                    PagoTipoPago = pago.Pago_TipoPago,
                    PagoFechaCreacion = pago.Pago_FechaCreacion,
                    PagoVigencia = pago.Pago_Vigencia,
                    PagoIdUsuarioCreador = pago.Pago_IdUsuarioCreador,
                    CajaId = pago.Pago_IdCaja,
                    EntidadId = 11
                };


                hospital.SaveChanges();

                Logger.Info($"El  Pago con el ID: {idPago} ha sido eliminado.");


                await SendMessageQueue(pagoEntities);
                Logger.Info($"El pago {pagoEntities.PagoReferencia} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
        #region INTEGRACION
        private async Task SendMessageQueue(PagoEntities pagoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(pagoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion

    }
}
