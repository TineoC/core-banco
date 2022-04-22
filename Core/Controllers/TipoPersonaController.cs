using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
///??? Para Hacer Operaciones
///// Funciona puede mejorar
namespace Core.Controllers
{
    internal class TipoPersonaController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(TipoPersona tipoPersona)
        {
            Console.WriteLine($"Identificacion del tipo de Persona: {tipoPersona.TipoPersona_Id}");
            Console.WriteLine($"Descripcion del tipo de Persona: {tipoPersona.TipoPersona_Descripcion}");
            Console.WriteLine($"Fecha de creacion: { tipoPersona.TipoPersona_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: { tipoPersona.TipoPersona_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: { tipoPersona.TipoPersona_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists = true;
                string descripcion;
                do
                {


                    Console.Write("Escribe la descripcion de tipo de persona: ");
                    descripcion = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.TipoPersona.Any(tipopers => tipopers.TipoPersona_Descripcion == descripcion);

                    if (exists)
                    {
                        Console.WriteLine("Existe un tipo de persona con esa descripcion");

                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (exists);

                hospital.TipoPersona.Add(new TipoPersona()
                {
                    TipoPersona_Descripcion = descripcion,
                    TipoPersona_FechaCreacion = DateTime.Now,
                    TipoPersona_IdUsuarioCreador = Program.loggerUserID,
                    TipoPersona_Vigencia = true
                });


                Logger.Info($"Se ha creado el tipo de Persona correctamente: {descripcion}");


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
            int TipoPersona;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de Persona: ");
                TipoPersona = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoPersona.Any(tipoPers => tipoPers.TipoPersona_Id == TipoPersona);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Personas con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            TipoPersona tipoPersona = hospital.TipoPersona
                            .Where(
                                tipoPers => tipoPers.TipoPersona_Id == TipoPersona
                            )
                            .FirstOrDefault();

            MostrarInformacion(tipoPersona);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (TipoPersona tipoPersona in hospital.TipoPersona.ToList())
            {
                Console.Clear();
                Console.WriteLine($"TipoPersona: {index}");

                MostrarInformacion(tipoPersona);
                Console.Write("Press any key to continue...");
                Console.ReadKey();

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int TipoPersona;
            string descripcion;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identifiacion del tipo de Persona  a actualizar: ");
                TipoPersona = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoPersona.Any(tipoPers => tipoPers.TipoPersona_Id == TipoPersona);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de Personas con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            do
            {
                exists = true;
                Console.Write("Escribe la descripcion del tipo de persona (actualizado): ");
                descripcion = Console.ReadLine();

                Console.Clear();

                exists = hospital.TipoPersona.Any(tipopers => tipopers.TipoPersona_Descripcion == descripcion);

                if (exists)
                {
                    Console.WriteLine("Existen tipos de personas con esa descripcion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (exists);


            TipoPersona nuevoTipoPersona = hospital.TipoPersona.Where(
                    tipoPers => tipoPers.TipoPersona_Id == TipoPersona
                ).First();

            nuevoTipoPersona.TipoPersona_Descripcion = descripcion;


            Logger.Info($"El tipo de Persona con la identificacion {TipoPersona} ha sido actualizado.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int TipoPersona;

                do
                {
                    Console.Write("Escribe la identificacion del tipo de Persona a eliminar: ");
                    TipoPersona = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoPersona.Any(tipoPers => tipoPers.TipoPersona_Id == TipoPersona);

                    if (!exists)
                    {
                        Console.WriteLine("No existen tipos de Personas con esa identifiacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.TipoPersona.Where(
                        tipopers => tipopers.TipoPersona_Id == TipoPersona
                ).First().TipoPersona_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"El tipo de Persona con la identifiacion {TipoPersona} ha sido eliminado.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
