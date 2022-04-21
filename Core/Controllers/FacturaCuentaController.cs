using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class FacturaCuentaController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(Cuenta_Factura cuentaFactura)
        {
            Console.WriteLine($"ID Factura: {cuentaFactura.Cuenta_Factura_IdFactura}");
            Console.WriteLine($"ID Cuenta: {cuentaFactura.Cuenta_Factura_IdCuenta}");
            Console.WriteLine($"Monto: {cuentaFactura.Cuenta_Factura_Monto}");
            Console.WriteLine($"Fecha Creación: {cuentaFactura.Cuenta_Factura_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {cuentaFactura.Cuenta_Factura_dUsuarioCreador}");
            Console.WriteLine($"Vigencia: {cuentaFactura.Cuenta_Factura_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.WriteLine("Escribe el ID de la Factura: ");
                int idFactura = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el ID de la Cuenta: ");
                int idCuenta = Int32.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el monto de la Factura: ");
                Decimal monto = Decimal.Parse(Console.ReadLine());

                hospital.Cuenta_Factura.Add(new Cuenta_Factura()
                {
                    Cuenta_Factura_IdFactura = idFactura,
                    Cuenta_Factura_IdCuenta = idCuenta,
                    Cuenta_Factura_Monto = monto,
                    Cuenta_Factura_FechaCreacion = DateTime.Now,
                    Cuenta_Factura_dUsuarioCreador = Program.loggerUserID,
                    Cuenta_Factura_Vigencia = true
                });

                hospital.SaveChanges();

                Logger.Info($"Se ha creado la Factura correctamente");
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
                int idFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura: ");

                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == idCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el monto de la Factura");
                Decimal monto = Decimal.Parse(Console.ReadLine());

                Console.Clear();

                Cuenta_Factura cuenta_Factura = hospital.Cuenta_Factura
                                .Where(
                                    cuent_fac => 
                                        cuent_fac.Cuenta_Factura_IdFactura == idFactura
                                        &&
                                        cuent_fac.Cuenta_Factura_IdCuenta == idCuenta
                                )
                                .FirstOrDefault();

                MostrarInformacion(cuenta_Factura);
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
                foreach (Cuenta_Factura cuenta_Factura in hospital.Cuenta_Factura.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Facturas_Cuenta: #{index}");

                    MostrarInformacion(cuenta_Factura);

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
            int id;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                int idFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura: ");

                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == idCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura (actualizado): ");

                    nuevoIdFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == nuevoIdFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

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

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == nuevoIdCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Cuenta_Factura cuenta_factura = hospital.Cuenta_Factura
                    .Where(
                    cuent_fac =>
                        cuent_fac.Cuenta_Factura_IdFactura == idFactura
                        &&
                        cuent_fac.Cuenta_Factura_IdCuenta == idCuenta
                    ).First();

                cuenta_factura.Cuenta_Factura_IdFactura = nuevoIdFactura;
                cuenta_factura.Cuenta_Factura_IdCuenta = nuevoIdCuenta;
                cuenta_factura.Cuenta_Factura_Vigencia = true;

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la Factura de la Cuenta correctamente.");
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

                int idFactura;

                do
                {
                    Console.Write("Escribe el id de la Factura: ");

                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuenta_Factura.Any(fac_cuenta => fac_cuenta.Cuenta_Factura_IdFactura == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idCuenta;

                do
                {
                    Console.Write("Escribe el id de la Cuenta: ");

                    idCuenta = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.Cuentas.Any(cuent => cuent.Cuenta_Id == idCuenta);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Cuenta con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.Cuenta_Factura.Where(
                    cuent_fac =>
                            cuent_fac.Cuenta_Factura_IdFactura == idFactura
                            &&
                            cuent_fac.Cuenta_Factura_IdCuenta == idCuenta
                    ).First().Cuenta_Factura_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado esta Factura con el ID: {idFactura} y la Cuenta ID: {idCuenta} correctamente.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }
    }
}
