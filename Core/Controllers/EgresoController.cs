﻿using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class EgresoController
    {
        static hospitalEntities hospital = new hospitalEntities();


        public static EgresoController Instancia = null;
        public static EgresoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new EgresoController();
            }

            return Instancia;
        }


        public static void MostrarInformacion(Egreso egreso)
        {
            Console.WriteLine($"ID Egreso: {egreso.Egreso_Id}");
            Console.WriteLine($"Pagado A: {egreso.Egreso_PagadoA}");
            Console.WriteLine($"Cédula: {egreso.Egreso_Cedula}");
            Console.WriteLine($"Monto: {egreso.Egreso_Monto}");
            Console.WriteLine($"Concepto: {egreso.Egreso_Concepto}");
            Console.WriteLine($"Preparado: {egreso.Egreso_Preparado}");
            Console.WriteLine($"Aprobado: {egreso.Egresp_Aprobado}");
            Console.WriteLine($"Recibido: {egreso.Egreso_Recibido}");
            Console.WriteLine($"Fecha Creación: {egreso.Egreso_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {egreso.Egreso_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {egreso.Egreso_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.WriteLine("Escribe a quien es pagado: ");
                string pagadoA = Console.ReadLine();

                Console.WriteLine("Escribe tu cédula: ");
                string cedula = Console.ReadLine();

                Console.WriteLine("Escribe el monto del egreso: ");
                Double monto = Double.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el concepto del egreso: ");
                string egreso = Console.ReadLine();

                Console.WriteLine("Escribe quien aprobó el egreso: ");
                string aprobadoPor = Console.ReadLine();

                Console.WriteLine("Escribe quien preparó el egreso: ");
                string preparadoPor = Console.ReadLine();

                Console.WriteLine("Escribe quien recibió el egreso: ");
                string recibidoPor = Console.ReadLine();

                Egreso egreso1 = new Egreso()
                {
                    Egreso_PagadoA = pagadoA,
                    Egreso_Cedula = cedula,
                    Egreso_Monto = monto,
                    Egreso_Concepto = egreso,
                    Egreso_Preparado = preparadoPor,
                    Egresp_Aprobado = aprobadoPor,
                    Egreso_Recibido = recibidoPor,
                    Egreso_FechaCreacion = DateTime.Now,
                    Egreso_IdUsuarioCreador = Program.loggerUserID,
                    Egreso_Vigencia = true
                };

                EgresoEntities egresoEntities = new EgresoEntities() {
                    EgresoPagadoA = egreso1.Egreso_PagadoA,
                    EgresoCedula = egreso1.Egreso_Cedula,
                    EgresoMonto = Convert.ToDecimal(egreso1.Egreso_Monto),
                    EgresoConcepto = egreso1.Egreso_Concepto,
                    EgresoPreparado = egreso1.Egreso_Preparado,
                    EgrespAprobado = egreso1.Egresp_Aprobado,
                    EgresoRecibido = egreso1.Egreso_Recibido,
                    EgresoFechaCreacion = egreso1.Egreso_FechaCreacion,
                    EgresoIdUsuarioCreador = egreso1.Egreso_IdUsuarioCreador,
                    EgresoVigencia = true,
                    EntidadId = 8
                };

                hospital.Egreso.Add(egreso1);

                hospital.SaveChanges();

                Logger.Info($"Se ha creado el Egreso correctamente");

                await SendMessageQueue(egresoEntities);
                Logger.Info($"Se ha enviado correctamente el egreso por {egresoEntities.EgresoCedula} pagado a {egresoEntities.EgresoPagadoA}, por un monto de {egresoEntities.EgresoMonto}, el dia {egresoEntities.EgresoFechaCreacion}");
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
                int idEgreso;

                do
                {
                    Console.Write("Escribe el id del Egreso: ");

                    idEgreso = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Egreso.Any(eg => eg.Egreso_Id == idEgreso);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Egreso con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                Egreso egreso = hospital.Egreso
                                .Where(
                                    eg => eg.Egreso_Id == idEgreso
                                )
                                .FirstOrDefault();

                MostrarInformacion(egreso);
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
                foreach (Egreso egreso in hospital.Egreso.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Egreso: #{index}");

                    MostrarInformacion(egreso);

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
                int idEgreso;

                do
                {
                    Console.Write("Escribe el id del Egreso: ");

                    idEgreso = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Egreso.Any(
                        egreso => egreso.Egreso_Id == idEgreso
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Egreso con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (!exists);

                Console.WriteLine("Escribe a quien es pagado (actualizado): ");
                string pagadoANuevo = Console.ReadLine();

                Console.WriteLine("Escribe tu cédula (actualizado): ");
                string cedulaNuevo = Console.ReadLine();

                Console.WriteLine("Escribe el monto del egreso (actualizado): ");
                Double montoNuevo = Double.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el concepto del egreso (actualizado): ");
                string conceptoNuevo = Console.ReadLine();

                Console.WriteLine("Escribe quien aprobó el egreso (actualizado): ");
                string aprobadoPorNuevo = Console.ReadLine();

                Console.WriteLine("Escribe quien preparó el egreso (actualizado): ");
                string preparadoPorNuevo = Console.ReadLine();

                Console.WriteLine("Escribe quien recibió el egreso (actualizado): ");
                string recibidoPorNuevo = Console.ReadLine();

                Egreso nuevoEgreso = hospital.Egreso
                    .Where(
                        egreso =>
                            egreso.Egreso_Id == idEgreso
                    ).First();

                nuevoEgreso.Egreso_PagadoA = pagadoANuevo;
                nuevoEgreso.Egreso_Cedula = cedulaNuevo;
                nuevoEgreso.Egreso_Monto = montoNuevo;
                nuevoEgreso.Egreso_Concepto = conceptoNuevo;
                nuevoEgreso.Egreso_Preparado = preparadoPorNuevo;
                nuevoEgreso.Egresp_Aprobado = aprobadoPorNuevo;
                nuevoEgreso.Egreso_Recibido = recibidoPorNuevo;
                nuevoEgreso.Egreso_FechaCreacion = DateTime.Now;
                nuevoEgreso.Egreso_IdUsuarioCreador = Program.loggerUserID;
                nuevoEgreso.Egreso_Vigencia = true;

                EgresoEntities egresoEntities = new EgresoEntities()
                {
                    EgresoPagadoA = nuevoEgreso.Egreso_PagadoA,
                    EgresoCedula = nuevoEgreso.Egreso_Cedula,
                    EgresoMonto = Convert.ToDecimal(nuevoEgreso.Egreso_Monto),
                    EgresoConcepto = nuevoEgreso.Egreso_Concepto,
                    EgresoPreparado = nuevoEgreso.Egreso_Preparado,
                    EgrespAprobado = nuevoEgreso.Egresp_Aprobado,
                    EgresoRecibido = nuevoEgreso.Egreso_Recibido,
                    EgresoFechaCreacion = nuevoEgreso.Egreso_FechaCreacion,
                    EgresoIdUsuarioCreador = nuevoEgreso.Egreso_IdUsuarioCreador,
                    EgresoVigencia = true,
                    EntidadId = 8
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado el Egreso ID:{nuevoEgreso.Egreso_Id}.");
                await SendMessageQueue(egresoEntities);
                Logger.Info($"Se ha enviado correctamente el egreso por {egresoEntities.EgresoCedula} pagado a {egresoEntities.EgresoPagadoA}, por un monto de {egresoEntities.EgresoMonto}, el dia {egresoEntities.EgresoFechaCreacion}");
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

                int idEgreso;

                do
                {
                    Console.Write("Escribe el id del Egreso: ");

                    idEgreso = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Egreso.Any(
                        egreso => egreso.Egreso_Id == idEgreso
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Egreso con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Egreso egresoEliminar = hospital.Egreso.Where(
                        egreso =>
                            egreso.Egreso_Id == idEgreso
                    ).First();
                egresoEliminar.Egreso_Vigencia = false;

                EgresoEntities egresoEntities = new EgresoEntities()
                {
                    EgresoPagadoA = egresoEliminar.Egreso_PagadoA,
                    EgresoCedula = egresoEliminar.Egreso_Cedula,
                    EgresoMonto = Convert.ToDecimal(egresoEliminar.Egreso_Monto),
                    EgresoConcepto = egresoEliminar.Egreso_Concepto,
                    EgresoPreparado = egresoEliminar.Egreso_Preparado,
                    EgrespAprobado = egresoEliminar.Egresp_Aprobado,
                    EgresoRecibido = egresoEliminar.Egreso_Recibido,
                    EgresoFechaCreacion = egresoEliminar.Egreso_FechaCreacion,
                    EgresoIdUsuarioCreador = egresoEliminar.Egreso_IdUsuarioCreador,
                    EgresoVigencia = egresoEliminar.Egreso_Vigencia,
                    EntidadId = 8
                };

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado el Egreso con el ID: {idEgreso}.");

                await SendMessageQueue(egresoEntities);
                Logger.Info($"Se ha enviado correctamente el egreso por {egresoEntities.EgresoCedula} pagado a {egresoEntities.EgresoPagadoA}, por un monto de {egresoEntities.EgresoMonto}, el dia {egresoEntities.EgresoFechaCreacion}");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }


        #region INTEGRACION
        private async Task SendMessageQueue(EgresoEntities egresoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(egresoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion

    }
}
