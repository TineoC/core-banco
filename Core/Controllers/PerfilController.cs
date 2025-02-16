﻿using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Funciona
namespace Core.Controllers
{
   internal class PerfilController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static PerfilController Instancia = null;
        public static PerfilController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new PerfilController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(Perfil perfil)
        {
            Console.WriteLine($"Identificacion: {perfil.Perfil_Id}");
            Console.WriteLine($"Descripcion: {perfil.Perfil_Descripcion}");
            Console.WriteLine($"Fecha Creación: {perfil.Perfil_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {perfil.Perfil_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {perfil.Perfil_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe la descripcion del perfil: ");
                string descripcion = Console.ReadLine();

                Perfil perfil = new Perfil() {
                  Perfil_Id = 0,
                  Perfil_Descripcion = descripcion,
                  Perfil_FechaCreacion = DateTime.Now,
                  Perfil_IdUsuarioCreador= Program.loggerUserID,
                  Perfil_Vigencia = true
                };


                    Console.Clear();
                hospital.Perfil.Add(perfil);

                    exists = hospital.Perfil.Any(perfil => perfil.Perfil_Descripcion == descripcion);

                    if (exists)
                    {
                        Console.WriteLine("Existe un perfil con esa descripcion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);



                hospital.Perfil.Add(new Perfil()
                {
                    Perfil_Descripcion = descripcion,
                    Perfil_FechaCreacion = DateTime.Now,
                    Perfil_IdUsuarioCreador = Program.loggerUserID,
                    Perfil_Vigencia = true
                }); ;
                PerfilEntities perfilEntities = new PerfilEntities()
                { PerfilId = perfil.Perfil_Id,
                  PerfilDescripcion = perfil.Perfil_Descripcion,
                  PerfilFechaCreacion = perfil.Perfil_FechaCreacion,
                  PerfilIdUsuarioCreador = perfil.Perfil_IdUsuarioCreador,
                  PerfilVigencia = true,
                  EntidadId = 12
                };

                Logger.Info($"Se ha creado la Perfil correctamente");

                hospital.SaveChanges();

                await SendMessageQueue(perfilEntities);
                Logger.Info($"El perfil {perfilEntities.PerfilDescripcion} se ha enviado correctamente");
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
            int perfilId;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el ID del Perfil a mostrar: ");
                perfilId = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Perfil.Any(per => per.Perfil_Id == perfilId);

                if (!exists)
                {
                    Console.WriteLine("No existe un  Perfil con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            Perfil Perfil = hospital.Perfil
                            .Where(
                                per => per.Perfil_Id == perfilId
                            )
                            .FirstOrDefault();

            MostrarInformacion(Perfil);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (Perfil perfil in hospital.Perfil.ToList())
            {
                Console.Clear();
                Console.WriteLine($"Perfil: {index}");

                MostrarInformacion(perfil);
                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int perfilid;
            string descripcion;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el ID del Perfil a actualizar: ");
                perfilid = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.Perfil.Any(pers => pers.Perfil_Id == perfilid);

                if (!exists)
                {
                    Console.WriteLine("No existe un  Perfil con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {
                exists = true;
                Console.Write("Escribe la descripcion del perfil (actualizada) ");
                descripcion = Console.ReadLine();

                Console.Clear();

                exists = hospital.Perfil.Any(perfil => perfil.Perfil_Descripcion == descripcion);

                if (exists)
                {
                    Console.WriteLine("Existe un perfil con esa descripcion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);



            Perfil nuevoPerfil = hospital.Perfil.Where(
                    pers => pers.Perfil_Id == perfilid
                ).First();

            nuevoPerfil.Perfil_Descripcion = descripcion;

            PerfilEntities perfilEntities = new PerfilEntities()
            {   PerfilId = nuevoPerfil.Perfil_Id,
                PerfilDescripcion = nuevoPerfil.Perfil_Descripcion,
                PerfilFechaCreacion = nuevoPerfil.Perfil_FechaCreacion,
                PerfilIdUsuarioCreador = nuevoPerfil.Perfil_IdUsuarioCreador,
                PerfilVigencia = true,
                EntidadId = 12
            };


            Logger.Info($"La Perfil con el ID {perfilid} ha sido actualizado.");
            hospital.SaveChanges();

            await SendMessageQueue(perfilEntities);
            Logger.Info($"El perfil {perfilEntities.PerfilDescripcion} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int perfilid;

                do
                {
                    Console.Write("Escribe el ID del Perfil a eliminar: ");
                    perfilid = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Perfil.Any(pers => pers.Perfil_Id == perfilid);

                    if (!exists)
                    {
                        Console.WriteLine("No existe un perfil  con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Perfil nuevoPerfil = hospital.Perfil.Where(
                         pers => pers.Perfil_Id == perfilid
                     ).First();

                nuevoPerfil.Perfil_Vigencia = false;

                PerfilEntities perfilEntities = new PerfilEntities()
                {
                    PerfilId = nuevoPerfil.Perfil_Id,
                    PerfilDescripcion = nuevoPerfil.Perfil_Descripcion,
                    PerfilFechaCreacion = nuevoPerfil.Perfil_FechaCreacion,
                    PerfilIdUsuarioCreador = nuevoPerfil.Perfil_IdUsuarioCreador,
                    PerfilVigencia = nuevoPerfil.Perfil_Vigencia,
                    EntidadId = 12
                };


                hospital.SaveChanges();
                Logger.Info($"El Perfil con el ID: {perfilid} ha sido eliminado.");

                await SendMessageQueue(perfilEntities);
                Logger.Info($"El perfil {perfilEntities.PerfilDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(PerfilEntities PerfilEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(PerfilEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
