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
   internal class PlanDeTratamientoController
    {
        public static PlanDeTratamientoController Instancia = null;
        public static PlanDeTratamientoController GetInstance()
        {
            if (Instancia == null)
            {
                Instancia = new PlanDeTratamientoController();
            }

            return Instancia;
        }
        static hospitalEntities hospital = new hospitalEntities();
        public static void MostrarInformacion(PlanDeTratamiento planDeTratamiento)
        {
            Console.WriteLine($"Identificacion del plan de tratamiento: {planDeTratamiento.PlanDeTratamiento_Id}");
            Console.WriteLine($"ID de procedimiento: {planDeTratamiento.PlanDeTratamiento_IdProcedimiento}");
            Console.WriteLine($"Numero de autorizacion: {planDeTratamiento.PlanDeTratamiento_NoAutorizacion}");
            Console.WriteLine($"ID del paciente: {planDeTratamiento.PlanDeTratamiento_IdPaciente}");
            Console.WriteLine($"ID del medico: {planDeTratamiento.PlanDeTratamiento_IdMedico}");
            Console.WriteLine($"Causa del tratamineto: {planDeTratamiento.PlanDeTratamiento_Causa}");
            Console.WriteLine($"Resultado: {planDeTratamiento.PlanDeTratamiento_Resultado}");
            Console.WriteLine($"Fecha de Creacion: {planDeTratamiento.PlanDeTratamiento_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {planDeTratamiento.PlanDeTratamiento_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {planDeTratamiento.PlanDeTratamiento_Vigencia}");
        }

        public async Task Crear()
        {
            bool exists = false;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
     
                string paciente, medico;
                int  prodemiento;

                do
                {
               
                    Console.Write("Escribe el ID del medico: ");
                    medico = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(plan => plan.Persona_Documento == medico);

                    if (!exists)
                    {
                        Console.WriteLine("No existe un medico con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                do
                {

                    Console.Write("Escribe el ID del paciente: ");
                   paciente = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(plan => plan.Persona_Documento == paciente);///??

                    if (!exists)
                    {
                        Console.WriteLine("No existe un  paciente con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                do
                {

                    Console.Write("Escribe el ID del procedimiento: ");
                   prodemiento = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.ProcesoMedico.Any(plan => plan.ProcesoMedico_Id == prodemiento);///??

                    if (!exists)
                    {
                        Console.WriteLine("No existe un procedimiento con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);


                Console.Write("Escribe la causa del tratamiento: ");
                string causa = Console.ReadLine();

                Console.Write("Escribe la resultado del tratamiento: ");
                string resultado = Console.ReadLine();

                Console.Write("Escribe el numero de autorizacion:  ");
                int autorizacion = Int32.Parse(Console.ReadLine());

                PlanDeTratamiento PlanDeTratamiento = new PlanDeTratamiento() {
                    PlanDeTratamiento_IdProcedimiento = prodemiento,
                    PlanDeTratamiento_NoAutorizacion = autorizacion,
                    PlanDeTratamiento_IdPaciente = paciente,
                    PlanDeTratamiento_IdMedico = medico,
                    PlanDeTratamiento_Causa = causa,
                    PlanDeTratamiento_Resultado = resultado,
                    PlanDeTratamiento_FechaCreacion = DateTime.Now,
                    PlanDeTratamiento_IdUsuarioCreador = Program.loggerUserID,
                    PlanDeTratamiento_Vigencia = true
                };

                hospital.PlanDeTratamiento.Add(PlanDeTratamiento);

                Logger.Info($"Se ha creado el  plan de tratamiento correctamente con el paciente :{paciente}");
                Console.WriteLine("Se ha creado el  plan de tratamiento correctamente.");

                hospital.SaveChanges();

                PlanTratamientoVigenciaEntities planDeTratamientoEntities = new PlanTratamientoVigenciaEntities() {
                    PlanDeTratamientoId = PlanDeTratamiento.PlanDeTratamiento_Id,
                    PlanDeTratamientoIdProcedimiento = PlanDeTratamiento.PlanDeTratamiento_IdProcedimiento,
                    PlanDeTratamientoNoAutorizacion = PlanDeTratamiento.PlanDeTratamiento_NoAutorizacion,
                    PlanDeTratamientoIdPaciente = PlanDeTratamiento.PlanDeTratamiento_IdPaciente,
                    PlanDeTratamientoIdMedico = PlanDeTratamiento.PlanDeTratamiento_IdMedico,
                    PlanDeTratamientoCausa = PlanDeTratamiento.PlanDeTratamiento_Causa,
                    PlanDeTratamientoResultado = PlanDeTratamiento.PlanDeTratamiento_Resultado,
                    PlanDeTratamientoFechaCreacion = DateTime.Now,
                    PlanDeTratamientoIdUsuarioCreador = Program.loggerUserID,
                    PlanDeTratamientoVigencia = true,
                    EntidadId = 14
                };

                
                await SendMessageQueue(planDeTratamientoEntities);
                Logger.Info($"El plan de tratamiento de numero de autorizacion {planDeTratamientoEntities.PlanDeTratamientoNoAutorizacion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }//verificar si existe
        }
        public static void Mostrar()
        {
            bool exists = false;
            int  PlanTratamiento;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identifiacion del  plan de tratamiento a mostrar: ");
                PlanTratamiento = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.PlanDeTratamiento.Any(plan => plan.PlanDeTratamiento_Id == PlanTratamiento);

                if (!exists)
                {
                    Logger.Error($"No existe un  plan de tratamientos con esa Identificacion: {PlanTratamiento}.");
                    Console.WriteLine("No existe un  plan de tratamientos con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            PlanDeTratamiento planDeTratamiento = hospital.PlanDeTratamiento
                            .Where(
                                plan => plan.PlanDeTratamiento_Id == PlanTratamiento
                            )
                            .FirstOrDefault();

            MostrarInformacion(planDeTratamiento);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (PlanDeTratamiento planDeTratamiento in hospital.PlanDeTratamiento.ToList())
            {
                Console.Clear();
                Console.WriteLine($"PlanDeTratamiento: {index}");

                MostrarInformacion(planDeTratamiento);

                index++;
            }
        }
        public async Task Actualizar()
        {
            bool exists = false;
            int PlanTratamiento;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del  plan de tratamiento a actualizar: ");
                PlanTratamiento = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.PlanDeTratamiento.Any(plan => plan.PlanDeTratamiento_Id == PlanTratamiento);

                if (!exists)
                {
                    Console.WriteLine("No existe un plan de tratamiento con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe el procedimiento (actualizado): ");
            int procedimiento = Int32.Parse(Console.ReadLine());

            Console.Write("Escribe el numero de autorizacion (actualizado): ");
            int autorizacion = Int32.Parse(Console.ReadLine());

            Console.Write("Escribe el ID del paciente (actualizado): ");
            string paciente = Console.ReadLine();

            Console.Write("Escribe el ID del medico (actualizado): ");
           string medico = Console.ReadLine();

            Console.Write("Escribe la nueva causa: ");
            string causa = Console.ReadLine();

            Console.Write("Escribe el nuevo resultado: ");
            string resultado = Console.ReadLine();

            PlanDeTratamiento nuevoPlanDeTratamiento = hospital.PlanDeTratamiento.Where(
                    plan => plan.PlanDeTratamiento_Id == PlanTratamiento
                ).First();

            nuevoPlanDeTratamiento.PlanDeTratamiento_IdProcedimiento = procedimiento;
            nuevoPlanDeTratamiento.PlanDeTratamiento_NoAutorizacion = autorizacion;
            nuevoPlanDeTratamiento.PlanDeTratamiento_IdPaciente = paciente;
            nuevoPlanDeTratamiento.PlanDeTratamiento_IdMedico = medico;
            nuevoPlanDeTratamiento.PlanDeTratamiento_Causa = causa;
            nuevoPlanDeTratamiento.PlanDeTratamiento_Resultado = resultado;

            PlanTratamientoVigenciaEntities planDeTratamientoEntities = new PlanTratamientoVigenciaEntities()
            {
                PlanDeTratamientoId = nuevoPlanDeTratamiento.PlanDeTratamiento_Id,
                PlanDeTratamientoIdProcedimiento = nuevoPlanDeTratamiento.PlanDeTratamiento_IdProcedimiento,
                PlanDeTratamientoNoAutorizacion = nuevoPlanDeTratamiento.PlanDeTratamiento_NoAutorizacion,
                PlanDeTratamientoIdPaciente = nuevoPlanDeTratamiento.PlanDeTratamiento_IdPaciente,
                PlanDeTratamientoIdMedico = nuevoPlanDeTratamiento.PlanDeTratamiento_IdMedico,
                PlanDeTratamientoCausa = nuevoPlanDeTratamiento.PlanDeTratamiento_Causa,
                PlanDeTratamientoResultado = nuevoPlanDeTratamiento.PlanDeTratamiento_Resultado,
                PlanDeTratamientoFechaCreacion = nuevoPlanDeTratamiento.PlanDeTratamiento_FechaCreacion,
                PlanDeTratamientoIdUsuarioCreador = nuevoPlanDeTratamiento.PlanDeTratamiento_IdUsuarioCreador,
                PlanDeTratamientoVigencia = true,
                EntidadId = 14
            };
            Logger.Info($"El PlanDeTratamiento con el ID:  {PlanTratamiento} ha sido actualizado.");

            hospital.SaveChanges();

            await SendMessageQueue(planDeTratamientoEntities);
            Logger.Info($"El plan de tratamiento de numero de autorizacion {planDeTratamientoEntities.PlanDeTratamientoNoAutorizacion} se ha enviado correctamente");

        }
        public async Task Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int PlanTratamiento;

                do
                {
                    Console.Write("Escribe el documento de la PlanDeTratamiento a eliminar: ");
                    PlanTratamiento = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.PlanDeTratamiento.Any(plan => plan.PlanDeTratamiento_Id == PlanTratamiento);

                    if (!exists)
                    {
                        Console.WriteLine("No existe un plan de tratamientos con esa identificacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                PlanDeTratamiento nuevoPlanDeTratamiento = hospital.PlanDeTratamiento.Where(
                        plan => plan.PlanDeTratamiento_Id == PlanTratamiento
                    ).First();

                nuevoPlanDeTratamiento.PlanDeTratamiento_Vigencia = false;

                PlanTratamientoVigenciaEntities planDeTratamientoEntities = new PlanTratamientoVigenciaEntities()
                {
                    PlanDeTratamientoId = nuevoPlanDeTratamiento.PlanDeTratamiento_Id,
                    PlanDeTratamientoIdProcedimiento = nuevoPlanDeTratamiento.PlanDeTratamiento_IdProcedimiento,
                    PlanDeTratamientoNoAutorizacion = nuevoPlanDeTratamiento.PlanDeTratamiento_NoAutorizacion,
                    PlanDeTratamientoIdPaciente = nuevoPlanDeTratamiento.PlanDeTratamiento_IdPaciente,
                    PlanDeTratamientoIdMedico = nuevoPlanDeTratamiento.PlanDeTratamiento_IdMedico,
                    PlanDeTratamientoCausa = nuevoPlanDeTratamiento.PlanDeTratamiento_Causa,
                    PlanDeTratamientoResultado = nuevoPlanDeTratamiento.PlanDeTratamiento_Resultado,
                    PlanDeTratamientoFechaCreacion = nuevoPlanDeTratamiento.PlanDeTratamiento_FechaCreacion,
                    PlanDeTratamientoIdUsuarioCreador = nuevoPlanDeTratamiento.PlanDeTratamiento_IdUsuarioCreador,
                    PlanDeTratamientoVigencia = nuevoPlanDeTratamiento.PlanDeTratamiento_Vigencia,
                    EntidadId = 14
                };

                hospital.SaveChanges();

                Logger.Info($"El plan de tratimiento con el ID:  {PlanTratamiento} ha sido eliminado.");

                await SendMessageQueue(planDeTratamientoEntities);
                Logger.Info($"El plan de tratamiento de numero de autorizacion {planDeTratamientoEntities.PlanDeTratamientoNoAutorizacion} se ha enviado correctamente");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }

        #region INTEGRACION
        private async Task SendMessageQueue(PlanTratamientoVigenciaEntities planDeTratamiento)
        {

            string queueName = "core";
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AzureServiceBus"].ConnectionString;
            var client = new QueueClient(connectionString, queueName, ReceiveMode.PeekLock);
            string messageBody = JsonConvert.SerializeObject(planDeTratamiento);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await client.SendAsync(message);
            await client.CloseAsync();
        }
        #endregion
    }
}
