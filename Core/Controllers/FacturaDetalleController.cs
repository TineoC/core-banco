using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class FacturaDetalleController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(FacturaDetalle facturaDetalle)
        {
            Console.WriteLine($"ID Detalle Factura: {facturaDetalle.FacturaDetalle_Id}");
            Console.WriteLine($"ID Factura: {facturaDetalle.FacturaDetalle_Id}");
            Console.WriteLine($"ID Plan de Tratamiento: {facturaDetalle.FacturaDetalle_PlanDeTratamiento}");
            Console.WriteLine($"Monto Bruto: {facturaDetalle.FacturaDetalle_MontoBruto}");
            Console.WriteLine($"Monto Cobertura: {facturaDetalle.FacturaDetalle_MontoCobertura}");
            Console.WriteLine($"Monto ITBIS: {facturaDetalle.FacturaDetalle_MontoItbis}");
            Console.WriteLine($"Monto Descuento: {facturaDetalle.FacturaDetalle_MontoDescuento}");
            Console.WriteLine($"Monto Total: {facturaDetalle.FacturaDetalle_MontoTotal}");
            Console.WriteLine($"Fecha Creación: {facturaDetalle.FacturaDetalle_FechaCreacion}");
            Console.WriteLine($"ID Usuario Creador: {facturaDetalle.FacturaDetalle_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {facturaDetalle.FacturaDetalle_Vigencia}");
        }

        public static void Crear()
        {
            bool exists = false;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                int idFactura;
                do
                {
                    Console.WriteLine("Escribe el ID de la Factura: ");
                    idFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaEncabezado.Any(
                        factura => factura.FacturaEncabezado_Id == idFactura
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ninguna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int idPlanDeTratamiento;
                do
                {
                    Console.WriteLine("Escribe el ID del Plan de Tratamiento: ");
                    idPlanDeTratamiento = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.PlanDeTratamiento.Any(
                        plan => plan.PlanDeTratamiento_Id == idPlanDeTratamiento
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Plan de Tratamiento con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el Monto Bruto: ");
                Decimal monto = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe la cobertura de la Factura: ");
                Decimal cobertura = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el ITBIS de la Factura: ");
                Decimal ITBIS = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Descuento de la Factura: ");
                Decimal Descuento = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total de la Factura: ");
                Decimal Total = Decimal.Parse(Console.ReadLine());

                hospital.FacturaDetalle.Add(new FacturaDetalle()
                {
                    FacturaDetalle_IdFactura = idFactura,
                    FacturaDetalle_PlanDeTratamiento = idPlanDeTratamiento,
                    FacturaDetalle_MontoBruto = monto,
                    FacturaDetalle_MontoCobertura = cobertura,
                    FacturaDetalle_MontoItbis = ITBIS,
                    FacturaDetalle_MontoDescuento = Descuento,
                    FacturaDetalle_MontoTotal = Total,
                    FacturaDetalle_FechaCreacion = DateTime.Now,
                    FacturaDetalle_IdUsuarioCreador = Program.loggerUserID,
                    FacturaDetalle_Vigencia = true
                });

                hospital.SaveChanges();

                Logger.Info($"Se ha creado el Detalle de la Factura correctamente");
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
                int idDetalleFactura;

                do
                {
                    Console.Write("Escribe el id del Detalle de la Factura: ");

                    idDetalleFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaDetalle.Any(fac_detalle => fac_detalle.FacturaDetalle_Id == idDetalleFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Detalle de Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                FacturaDetalle detalleFactura = hospital.FacturaDetalle
                                .Where(
                                    fac_detalle => fac_detalle.FacturaDetalle_Id == idDetalleFactura
                                )
                                .FirstOrDefault();

                MostrarInformacion(detalleFactura);
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
                foreach (FacturaDetalle egreso in hospital.FacturaDetalle.ToList())
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
        public static void Actualizar()
        {
            bool exists = false;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                int idDetalleFactura;
                do
                {
                    Console.WriteLine("Escribe el ID del Detalle de la Factura: ");
                    idDetalleFactura = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaDetalle.Any(
                        factura => factura.FacturaDetalle_Id == idDetalleFactura
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Detalle de Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdPlanDeTratamiento;
                do
                {
                    Console.WriteLine("Escribe el ID del Plan de Tratamiento (actualizado): ");
                    nuevoIdPlanDeTratamiento = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.PlanDeTratamiento.Any(
                        plan => plan.PlanDeTratamiento_Id == nuevoIdPlanDeTratamiento
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Plan de Tratamiento con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el Monto Bruto (actualizado): ");
                Decimal montoNuevo = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe la cobertura de la Factura (actualizado): ");
                Decimal coberturaNuevo = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el ITBIS de la Factura (actualizado): ");
                Decimal ITBISNuevo = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Descuento de la Factura (actualizado): ");
                Decimal DescuentoNuevo = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total de la Factura (actualizado): ");
                Decimal TotalNuevo = Decimal.Parse(Console.ReadLine());

                FacturaDetalle nuevoDetalle = hospital.FacturaDetalle
                    .Where(
                        detalle =>
                            detalle.FacturaDetalle_Id == idDetalleFactura
                    ).First();

                nuevoDetalle.FacturaDetalle_PlanDeTratamiento = nuevoIdPlanDeTratamiento;
                nuevoDetalle.FacturaDetalle_MontoBruto = montoNuevo;
                nuevoDetalle.FacturaDetalle_MontoCobertura = coberturaNuevo;
                nuevoDetalle.FacturaDetalle_MontoItbis = ITBISNuevo;
                nuevoDetalle.FacturaDetalle_MontoDescuento = DescuentoNuevo;
                nuevoDetalle.FacturaDetalle_MontoTotal = TotalNuevo;
                nuevoDetalle.FacturaDetalle_FechaCreacion = DateTime.Now;
                nuevoDetalle.FacturaDetalle_IdUsuarioCreador = Program.loggerUserID;
                nuevoDetalle.FacturaDetalle_Vigencia = true;

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado el Egreso ID:{nuevoDetalle.FacturaDetalle_Id}.");
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

                int idDetalle;

                do
                {
                    Console.Write("Escribe el id del Detalle de las Facturas: ");

                    idDetalle = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.FacturaDetalle.Any(
                        detalle => detalle.FacturaDetalle_Id == idDetalle
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Detalle de Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.FacturaDetalle.Where(
                        detalle => detalle.FacturaDetalle_Id == idDetalle
                    ).First().FacturaDetalle_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado el Detalle de Factura con el ID: {idDetalle}.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
