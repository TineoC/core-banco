using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DTO;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
//Cambiar
//Funciona
namespace Core.Controllers
{
   internal class ProcesoMedicoController
    {

        static hospitalEntities hospital = new hospitalEntities();
        public static ProcesoMedicoController Instancia = null;
        public static ProcesoMedicoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new ProcesoMedicoController();
            }

            return Instancia;
        }

        public static void MostrarInformacion(ProcesoMedico procesoMedico)
        {
            Console.WriteLine($"Identifiacion del proceso medico: {procesoMedico.ProcesoMedico_Id}");
            Console.WriteLine($"Descripcion: {procesoMedico.ProcesoMedico_Descripcion}");
            Console.WriteLine($"Precio: {procesoMedico.ProcesoMedico_Precio}");
            Console.WriteLine($"ID del tipo de proceso: {procesoMedico.ProcesoMedico_IdTipoProceso}");
            Console.WriteLine($"Fecha Creación: {procesoMedico.ProcesoMedico_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {procesoMedico.ProcesoMedico_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {procesoMedico.ProcesoMedico_Vigencia}");
        }

        public async Task Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists = false;
                int tipoproceso;
                string descripcion;

                do
                {
                    exists = true;
                    Console.Write("Escribe la descripcion del proceso medico: ");
                    descripcion = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.ProcesoMedico.Any(proMed => proMed.ProcesoMedico_Descripcion == descripcion);

                    if (exists)
                    {
                        Console.WriteLine("Existe un proceso medico con esa descripcion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);

                do
                {
                    Console.Write("Escribe el ID del tipo de proceso: ");
                    tipoproceso = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoProceso.Any(procMed => procMed.TipoProceso_Id == tipoproceso);

                    if (!exists)
                    {
                        Console.WriteLine("No existe un tipo de proceso con esa identificacion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                decimal precio;

                do
                {
                    Console.Write("Escribe el precio: ");
                    precio = Decimal.Parse(Console.ReadLine());

                    if(precio <= 0)
                    {
                        Console.WriteLine("Error. El precio no puede ser menor o igual a cero");
                        Logger.Error("Error. El precio no puede ser menor o igual a cero");
                    }
                } while (precio <= 0);         

                ProcesoMedico procesoMedico = new ProcesoMedico()
                {
                    ProcesoMedico_Descripcion = descripcion,
                    ProcesoMedico_Precio = precio,
                    ProcesoMedico_FechaCreacion = DateTime.Now,
                    ProcesoMedico_IdUsuarioCreador = Program.loggerUserID,
                    ProcesoMedico_Vigencia = true,
                    ProcesoMedico_IdTipoProceso = tipoproceso
                };

                hospital.ProcesoMedico.Add(procesoMedico);

                ProcesoMedicoEntities procesoMedico1 = new ProcesoMedicoEntities()
                {
                    ProcesoMedicoId = procesoMedico.ProcesoMedico_Id,
                    ProcesoMedicoDescripcion = procesoMedico.ProcesoMedico_Descripcion,
                    ProcesoMedicoPrecio = procesoMedico.ProcesoMedico_Precio,
                    ProcesoMedicoFechaCreacion = procesoMedico.ProcesoMedico_FechaCreacion,
                    ProcesoMedicoIdUsuarioCreador = procesoMedico.ProcesoMedico_IdUsuarioCreador,
                    ProcesoMedicoVigencia = true,
                    ProcesoMedicoIdTipoProceso = procesoMedico.ProcesoMedico_IdTipoProceso
                };

                Logger.Info($"Se ha creado el proceso Medico correctamente.");
                Console.WriteLine($"Se ha creado el proceso Medico correctamente.");

                hospital.SaveChanges();

                await SendMessageQueue(procesoMedico1);
                Logger.Info($"El proceso medico {procesoMedico1.ProcesoMedicoDescripcion} se ha enviado correctamente");
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
           int ProcesoMedicoId,TipoProcesoId;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identifiacion del proceso medico: ");
                ProcesoMedicoId  = Int32.Parse(Console.ReadLine());


                Console.Clear();

                exists = hospital.ProcesoMedico.Any(procMed => procMed.ProcesoMedico_Id == ProcesoMedicoId);

                if (!exists)
                {
                    Console.WriteLine("No existen proceso medicos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);
            

            Console.Clear();

            ProcesoMedico procesoMedico = hospital.ProcesoMedico
                            .Where(
                                per => per.ProcesoMedico_Id == ProcesoMedicoId
                            )
                            .FirstOrDefault();

            MostrarInformacion(procesoMedico);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (ProcesoMedico procesoMedico in hospital.ProcesoMedico.ToList())
            {
                Console.Clear();
                Console.WriteLine($"ProcesoMedico: {index}");

                MostrarInformacion(procesoMedico);

                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int ProcesoMedicoId, TipoProcesoId;
            string descripcion;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identifiacion del proceso medico a actualizar: ");
                ProcesoMedicoId = Int32.Parse(Console.ReadLine());
                Console.Clear();

                exists = hospital.ProcesoMedico.Any(procMed => procMed.ProcesoMedico_Id == ProcesoMedicoId);

                if (!exists)
                {
                    Console.WriteLine("No existen proceso medicos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);
            do
            {
                Console.Write("Escribe el ID del tipo de proceso (actualizada): ");
                TipoProcesoId = Int32.Parse(Console.ReadLine());


                Console.Clear();

                exists = hospital.TipoProceso.Any(procMed => procMed.TipoProceso_Id == TipoProcesoId);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de procesos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {
                exists = true;
                Console.Write("Escribe la descripcion del proceso medico (actualizado): ");
                descripcion = Console.ReadLine();

                Console.Clear();

                exists = hospital.ProcesoMedico.Any(procMed => procMed.ProcesoMedico_Descripcion == descripcion);

                if (exists)
                {
                    Console.WriteLine("Existen procesos medicos con esa descripcion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);

            Console.Write("Escribe el precio (actualizado) : ");
            decimal precio = Decimal.Parse(Console.ReadLine());

        

            ProcesoMedico nuevoProcesoMedico = hospital.ProcesoMedico.Where(
                    procMed => procMed.ProcesoMedico_Id == ProcesoMedicoId
                ).First();

            nuevoProcesoMedico.ProcesoMedico_Descripcion = descripcion;
            nuevoProcesoMedico.ProcesoMedico_Precio = precio;
            nuevoProcesoMedico.ProcesoMedico_IdTipoProceso = TipoProcesoId;

            ProcesoMedicoEntities procesoMedico1 = new ProcesoMedicoEntities()
            {
                ProcesoMedicoId = nuevoProcesoMedico.ProcesoMedico_Id,
                ProcesoMedicoDescripcion = nuevoProcesoMedico.ProcesoMedico_Descripcion,
                ProcesoMedicoPrecio = nuevoProcesoMedico.ProcesoMedico_Precio,
                ProcesoMedicoFechaCreacion = nuevoProcesoMedico.ProcesoMedico_FechaCreacion,
                ProcesoMedicoIdUsuarioCreador = nuevoProcesoMedico.ProcesoMedico_IdUsuarioCreador,
                ProcesoMedicoVigencia = true,
                ProcesoMedicoIdTipoProceso = nuevoProcesoMedico.ProcesoMedico_IdTipoProceso
            };

            Logger.Info($"El proceso Medico  ha sido actualizado correctamente con la identifiacion {ProcesoMedicoId}");
          
            hospital.SaveChanges();

            await SendMessageQueue(procesoMedico1);
            Logger.Info($"El proceso medico {procesoMedico1.ProcesoMedicoDescripcion} se ha enviado correctamente");
        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {

                bool exists = false;
                int ProcesoMedicoId;

                do
                {
                    Console.Write("Escribe la identifiacion del proceso medico a eliminar: ");
                    ProcesoMedicoId = Int32.Parse(Console.ReadLine());


                    Console.Clear();

                    exists = hospital.ProcesoMedico.Any(procMed => procMed.ProcesoMedico_Id == ProcesoMedicoId);

                    if (!exists)
                    {
                        Console.WriteLine("No existen ProcesoMedicos con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);


                ProcesoMedico nuevoProcesoMedico = hospital.ProcesoMedico.Where(
                        procMed => procMed.ProcesoMedico_Id == ProcesoMedicoId
                    ).First();

                nuevoProcesoMedico.ProcesoMedico_Vigencia = false;

                ProcesoMedicoEntities procesoMedico1 = new ProcesoMedicoEntities()
                {
                    ProcesoMedicoId = nuevoProcesoMedico.ProcesoMedico_Id,
                    ProcesoMedicoDescripcion = nuevoProcesoMedico.ProcesoMedico_Descripcion,
                    ProcesoMedicoPrecio = nuevoProcesoMedico.ProcesoMedico_Precio,
                    ProcesoMedicoFechaCreacion = nuevoProcesoMedico.ProcesoMedico_FechaCreacion,
                    ProcesoMedicoIdUsuarioCreador = nuevoProcesoMedico.ProcesoMedico_IdUsuarioCreador,
                    ProcesoMedicoVigencia = nuevoProcesoMedico.ProcesoMedico_Vigencia,
                    ProcesoMedicoIdTipoProceso = nuevoProcesoMedico.ProcesoMedico_IdTipoProceso
                };

                hospital.SaveChanges();

                Logger.Info($"El proceso Medico  ha sido eliminado correctamente con la identifiacion {ProcesoMedicoId}");

                await SendMessageQueue(procesoMedico1);
                Logger.Info($"El proceso medico {procesoMedico1.ProcesoMedicoDescripcion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(ProcesoMedicoEntities ProcesoMedicoEntities)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(ProcesoMedicoEntities);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
