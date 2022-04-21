using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// /?
/// </summary>
namespace Core.Controllers
{
   internal class TipoNCFController
    {
        
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(TipoNCF tipoNCF)
        {
            Console.WriteLine($"Descripcion del tipo de NCF: {tipoNCF.TipoNCF_Descripcion}");
            Console.WriteLine($"ID del usuario: { tipoNCF.TipoNCF_IdUsuario}");
            Console.WriteLine($"Fecha de Creacion: { tipoNCF.TipoNCF_FechaCreado}");
            Console.WriteLine($"Estado: { tipoNCF.TipoNCF_Estado}"); 
            Console.WriteLine($"ID de la sucural: { tipoNCF.TipoNCF_IdSucursal}");

        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe la descripcion de tipo de NCF: ");
                string descripcion = Console.ReadLine();

                Console.Write("Escribe el ID usuario de tipo de NCF: ");
                int usuario = Int32.Parse(Console.ReadLine());

                Console.Write("Escribe el estado: ");
                bool Estado = bool.Parse(Console.ReadLine());

                Console.Write("Escribe el ID de la sucursal: ");
                int sucursal = Int32.Parse(Console.ReadLine());

                hospital.TipoNCF.Add(new TipoNCF()
                {
                    TipoNCF_Descripcion = descripcion,
                    TipoNCF_IdUsuario = usuario,
                    TipoNCF_Estado = Estado,
                    TipoNCF_IdSucursal = sucursal

                }); 


                Logger.Info($"Se ha creado el tipo de NCF correctamente");


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
        
            int tipoNCFid;
           
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de NCF: ");
                tipoNCFid = Int32.Parse(Console.ReadLine());


                Console.Clear();

                exists = hospital.TipoNCF.Any(tipopro => tipopro.TipoNCF_Id == tipoNCFid );

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de NCFs con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            TipoNCF tipoNCF = hospital.TipoNCF
                            .Where(
                                tipopro => tipopro.TipoNCF_Id == tipoNCFid
                            )
                            .FirstOrDefault();

            MostrarInformacion(tipoNCF);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (TipoNCF tipoNCF in hospital.TipoNCF.ToList())
            {
                Console.Clear();
                Console.WriteLine($"TipoNCF: {index}");

                MostrarInformacion(tipoNCF);

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
      
            int  tipoNCFid;
           
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe la identificacion del tipo de NCF  a actualizar: ");
                tipoNCFid = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.TipoNCF.Any(tipopro => tipopro.TipoNCF_Id == tipoNCFid);

                if (!exists)
                {
                    Console.WriteLine("No existen tipos de NCFs con esa identificacion");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe la descripcion de tipo de NCF (actualizada): ");
            string descripcion = Console.ReadLine();

            Console.Write("Escribe el ID usuario de tipo de NCF (actualizada): ");
            int usuario = Int32.Parse(Console.ReadLine());

            Console.Write("Escribe el estado (actualizado): ");
            bool Estado = bool.Parse(Console.ReadLine());

            Console.Write("Escribe el ID de la sucursal (actualizado): ");
            int sucursal = Int32.Parse(Console.ReadLine());
           


            TipoNCF nuevoTipoNCF = hospital.TipoNCF.Where(
                    tipopro => tipopro.TipoNCF_Id == tipoNCFid
                ).First();

            nuevoTipoNCF.TipoNCF_Descripcion = descripcion;
            nuevoTipoNCF.TipoNCF_IdUsuario = usuario;
            nuevoTipoNCF.TipoNCF_Estado = Estado;
            nuevoTipoNCF.TipoNCF_IdSucursal = sucursal;


            Logger.Info($"El tipo de NCF con la identificacion {tipoNCFid} ha sido actualizado.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int TipoNCF;

                do
                {
                    Console.Write("Escribe la identifiacion del tipo de NCF a eliminar: ");
                    TipoNCF = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.TipoNCF.Any(tipopro => tipopro.TipoNCF_Id == TipoNCF);

                    if (!exists)
                    {
                        Console.WriteLine("No existen tipos de NCFs con esa identifiacion");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.TipoNCF.Remove(hospital.TipoNCF.Where(
                        tipopro => tipopro.TipoNCF_Id == TipoNCF
                    ).First()
                );

                hospital.SaveChanges();

                Logger.Info($"El tipo de NCF con la identifiacion {TipoNCF} ha sido eliminado.");
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
