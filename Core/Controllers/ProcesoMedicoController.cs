using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//Cambiar
//Funciona
namespace Core.Controllers
{
   internal class ProcesoMedicoController
    {

        static hospitalEntities hospital = new hospitalEntities();
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

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists = false;
                int tipoproceso;
                Console.Write("Escribe la descripcion del proceso medico: ");
                string descripcion = Console.ReadLine();

                Console.Write("Escribe el precio: ");
                decimal precio = Decimal.Parse(Console.ReadLine());

                Console.Write("Escribe el ID del tipo de proceso: ");
                tipoproceso = Int32.Parse(Console.ReadLine());

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

                hospital.ProcesoMedico.Add(new ProcesoMedico()
                {
                    ProcesoMedico_Descripcion = descripcion,
                    ProcesoMedico_Precio = precio,
                    ProcesoMedico_IdTipoProceso = tipoproceso
                });

                Logger.Info($"Se ha creado el proceso Medico correctamente con la descripccion {descripcion}");

                hospital.SaveChanges();
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

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int ProcesoMedicoId, TipoProcesoId;
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

            Console.Write("Escribe la descripcion del proceso medico (actualizado): ");
            string descripcion = Console.ReadLine();

            Console.Write("Escribe el precio (actualizado) : ");
            decimal precio = Decimal.Parse(Console.ReadLine());

        

            ProcesoMedico nuevoProcesoMedico = hospital.ProcesoMedico.Where(
                    procMed => procMed.ProcesoMedico_Id == ProcesoMedicoId
                ).First();

            nuevoProcesoMedico.ProcesoMedico_Descripcion = descripcion;
            nuevoProcesoMedico.ProcesoMedico_Precio = precio;
            nuevoProcesoMedico.ProcesoMedico_IdTipoProceso = TipoProcesoId;

            Logger.Info($"El proceso Medico  ha sido actualizado correctamente con la identifiacion {ProcesoMedicoId}");
          
            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {

                bool exists = false;
                int ProcesoMedicoId, TipoProcesoId;
                

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
               

                hospital.ProcesoMedico.Remove(hospital.ProcesoMedico.Where(
                        procMed => procMed.ProcesoMedico_Id == ProcesoMedicoId
                    ).First()
                );

                hospital.SaveChanges();

                Logger.Info($"El proceso Medico  ha sido eliminado correctamente con la identifiacion {ProcesoMedicoId}");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
