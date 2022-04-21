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

        public static void MostrarInformacion(Cuentas cuenta)
        {
            Console.WriteLine($"ID Cuenta: {cuenta.Cuenta_Id}");
            Console.WriteLine($"ID Persona: {cuenta.Cuenta_IdPersona}");
            Console.WriteLine($"Balance: {cuenta.Cuenta_Balance}");
            Console.WriteLine($"Fecha Creación: {cuenta.Cuenta_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {cuenta.Cuenta_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {cuenta.Cuenta_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                bool exists;
                string documento;

                do
                {
                    Console.WriteLine("Escribe el documento de la Persona");
                    documento = Console.ReadLine();

                    exists = hospital.Persona.Any(
                        person => person.Persona_Documento == documento);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }

                } while (true);

                decimal balance;

                do
                {
                    Console.WriteLine("Escribe el Balance de la Cuenta");
                    balance = decimal.Parse(Console.ReadLine());
                } while (!(balance < 1));
                

                hospital.Cuentas.Add(new Cuentas()
                {
                    Cuenta_IdPersona = documento,
                    Cuenta_Balance = balance,
                    Cuenta_FechaCreacion = DateTime.Now,
                    Cuenta_IdUsuarioCreador = Program.loggerUserID,
                    Cuenta_Vigencia = true
                });

                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Cuenta correctamente");
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
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

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
        public static void Actualizar()
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

                int nuevoIdCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta (actualizado): ");

                    nuevoIdCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(
                        cuenta => cuenta.Cuenta_Id == nuevoIdCuenta
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                string nuevoDocumentoPersona;

                do
                {
                    Console.Write("Escribe el documento de la Cuenta (actualizado): ");

                    nuevoDocumentoPersona = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(
                        persona => persona.Persona_Documento == nuevoDocumentoPersona
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Persona con ese documento");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                decimal nuevoBalance;

                do
                {
                    Console.WriteLine("Escribe el Balance de la Cuenta");
                    nuevoBalance = decimal.Parse(Console.ReadLine());
                } while (!(nuevoBalance < 1));

                Cuentas nuevaCuenta = hospital.Cuentas
                    .Where(
                        cuenta =>
                            cuenta.Cuenta_Id == nuevoIdCuenta
                    ).First();

                nuevaCuenta.Cuenta_Id = nuevoIdCuenta;
                nuevaCuenta.Cuenta_IdPersona = nuevoDocumentoPersona;
                nuevaCuenta.Cuenta_Balance = nuevoBalance;
                nuevaCuenta.Cuenta_Vigencia = true;

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la Cuenta ID:{nuevaCuenta.Cuenta_Id} a NuevoID:{nuevoIdCuenta}.");
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

                hospital.Cuentas.Where(
                    cuent =>
                            cuent.Cuenta_Id == idCuenta
                    ).First().Cuenta_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Cuenta con el ID: {idCuenta}.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }
    }
}
