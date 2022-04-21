using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class UsuarioCajaController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(Caja_Usuario usuarioCaja)
        {
            Console.WriteLine($"ID Usuario: {usuarioCaja.Caja_Usuario_IdUsuario}");
            Console.WriteLine($"ID Caja: {usuarioCaja.Caja_Usuario_IdCaja}");
            Console.WriteLine($"Fecha Creación: {usuarioCaja.Caja_Usuario_IdUsuario_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {usuarioCaja.Caja_Usuario_IdUsuario_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {usuarioCaja.Caja_Usuario_IdUsuario_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists;
                int idUsuario;

                do
                {
                    Console.Write("Escribe el id del usuario de Caja: ");

                    idUsuario = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Usuarios.Any(user => user.Usuario_Id == idUsuario);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Usuario con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCaja;

                do
                {
                    Console.Write("Escribe el id de la Caja: ");

                    idCaja = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(user => user.Caja_Id == idCaja);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Usuario con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.Caja_Usuario.Add(new Caja_Usuario()
                {
                    Caja_Usuario_IdUsuario = idUsuario,
                    Caja_Usuario_IdCaja = idCaja,
                    Caja_Usuario_IdUsuario_FechaCreacion = DateTime.Now,
                    Caja_Usuario_IdUsuario_IdUsuarioCreador = Program.loggerUserID,
                    Caja_Usuario_IdUsuario_Vigencia = true
                });

                hospital.SaveChanges();

                Logger.Info($"Se ha creado el Usuario de Caja correctamente");
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
            int idUsuario;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                do
                {
                    Console.Write("Escribe el id del Usuario de Caja: ");

                    idUsuario = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(user => user.Caja_Usuario_IdUsuario == idUsuario);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Usuario de Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCaja;

                do
                {
                    Console.Write("Escribe el id de la Caja: ");

                    idCaja = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(user => user.Caja_Id == idCaja);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                Caja_Usuario usuarioCaja = hospital.Caja_Usuario
                                .Where(
                                    user => (
                                        user.Caja_Usuario_IdUsuario == idUsuario 
                                        &&                               
                                        user.Caja_Usuario_IdCaja == idCaja
                                        )
                                )
                                .FirstOrDefault();

                MostrarInformacion(usuarioCaja);
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
                foreach (Caja_Usuario usuarioCaja in hospital.Caja_Usuario.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Caja: #{index}");

                    MostrarInformacion(usuarioCaja);

                    Logger.Info($"Se ha mostrado el usuario de Caja #{index}");

                    index++;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                int idUsuario;

                do
                {
                    Console.Write("Escribe el id del Usuario de Caja: ");

                    idUsuario = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(user => user.Caja_Usuario_IdUsuario == idUsuario);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Usuario de Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCaja;

                do
                {
                    Console.Write("Escribe el id de la Caja: ");

                    idCaja = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(user => user.Caja_Id == idCaja);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdUsuario;

                do
                {
                    Console.Write("Escribe el nuevo id del Usuario de Caja: ");

                    nuevoIdUsuario = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(user => user.Caja_Usuario_IdUsuario == idUsuario);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Usuario de Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdCaja;

                do
                {
                    Console.Write("Escribe el nuevo id de la Caja: ");

                    nuevoIdCaja = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(user => user.Caja_Id == nuevoIdCaja);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Caja_Usuario nuevoUsuarioCaja = hospital.Caja_Usuario
                    .Where(
                        user =>
                            user.Caja_Usuario_IdUsuario == idUsuario
                            &&
                            user.Caja_Usuario_IdCaja == idCaja
                    ).First();

                nuevoUsuarioCaja.Caja_Usuario_IdUsuario = nuevoIdUsuario;
                nuevoUsuarioCaja.Caja_Usuario_IdCaja = nuevoIdCaja;
                nuevoUsuarioCaja.Caja_Usuario_IdUsuario_Vigencia = true;

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado el Usuario de Caja correctamente.");
            }
            catch (Exception e)
            {
                Logger.Error(e, "Ha ocurrido un error inesperado");
                throw;
            }    

        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;

                int idUsuario;

                do
                {
                    Console.Write("Escribe el id del Usuario de Caja: ");

                    idUsuario = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(user => user.Caja_Usuario_IdUsuario == idUsuario);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Usuario de Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCaja;

                do
                {
                    Console.Write("Escribe el id de la Caja: ");

                    idCaja = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Caja.Any(user => user.Caja_Id == idCaja);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.Caja_Usuario.Where(
                    user =>
                        user.Caja_Usuario_IdUsuario == idUsuario
                        &&
                        user.Caja_Usuario_IdCaja == idCaja
                    ).First().Caja_Usuario_IdUsuario_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado el Usuario de Caja ID: {idCaja} correctamente.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }
    }
}
