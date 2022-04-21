using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class FacturaEncabezadoController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(FacturaEncabezado facturaEncabezado)
        {
            Console.WriteLine($"ID Factura: {facturaEncabezado.FacturaEncabezado_Id}");
            Console.WriteLine($"ID NCF: {facturaEncabezado.FacturaEncabezado_IdNCF}");
            Console.WriteLine($"ID Cliente: {facturaEncabezado.FacturaEncabezado_IdCliente}");
            Console.WriteLine($"ID Cajero: {facturaEncabezado.FacturaEncabezado_IdCajero}");
            Console.WriteLine($"Total Bruto: {facturaEncabezado.FacturaEncabezado_TotalBruto}");
            Console.WriteLine($"Total Cobertura: {facturaEncabezado.FacturaEncabezado_TotalCobertura}");
            Console.WriteLine($"Total ITBIS: {facturaEncabezado.FacturaEncabezado_TotalItbis}");
            Console.WriteLine($"Total Descuento: {facturaEncabezado.FacturaEncabezado_TotalDescuento}");
            Console.WriteLine($"Total General: {facturaEncabezado.FacturaEncabezado_TotalGeneral}");
            Console.WriteLine($"Fecha Creación: {facturaEncabezado.FacturaEncabezado_FechaCreacion}");
            Console.WriteLine($"Id Usuario Creador: {facturaEncabezado.FacturaEncabezado_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {facturaEncabezado.FacturaEncabezado_Vigencia}");
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

                int idNCF;

                do
                {
                    Console.WriteLine("Escribe el ID del NCF: ");
                    idNCF = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.NCF.Any(
                        ncf => ncf.Id == idNCF
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Plan de Tratamiento con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escriba su NCF: ");
                string NCF = Console.ReadLine();

                string documentoCliente;

                do
                {
                    Console.WriteLine("Escribe el documento del Cliente: ");
                    documentoCliente = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(
                        persona => persona.Persona_Documento == documentoCliente
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cliente con ese documento");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                string cajeroID;

                do
                {
                    Console.WriteLine("Escribe el ID del Cajero: ");
                    cajeroID = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(
                        caj => Equals(caj.Caja_Usuario_IdCaja, cajeroID)
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cajero con ese ID");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el Total Bruto: ");
                Decimal totalBruto = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Cobertura: ");
                Decimal totalCobertura = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total ITBIS: ");
                Decimal totalITBIS = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Descuento: ");
                Decimal totalDescuento = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total General: ");
                Decimal totalGeneral = Decimal.Parse(Console.ReadLine());

                hospital.FacturaEncabezado.Add(new FacturaEncabezado()
                {
                    FacturaEncabezado_IdNCF = idNCF,
                    FacturaEncabezado_NCF = NCF,
                    FacturaEncabezado_IdCliente = documentoCliente,
                    FacturaEncabezado_IdCajero = cajeroID,
                    FacturaEncabezado_TotalBruto = totalBruto,
                    FacturaEncabezado_TotalCobertura = totalCobertura,
                    FacturaEncabezado_TotalItbis = totalITBIS,
                    FacturaEncabezado_TotalDescuento = totalDescuento,
                    FacturaEncabezado_TotalGeneral = totalGeneral,
                    FacturaEncabezado_FechaCreacion = DateTime.Now,
                    FacturaEncabezado_IdUsuarioCreador = Program.loggerUserID,
                    FacturaEncabezado_Vigencia = true
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

                    exists = hospital.FacturaEncabezado.Any(fac => fac.FacturaEncabezado_Id == idFactura);

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningúna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.Clear();

                FacturaEncabezado facturaEncabezado = hospital.FacturaEncabezado
                                .Where(
                                    fac => fac.FacturaEncabezado_Id == idFactura
                                )
                                .FirstOrDefault();

                MostrarInformacion(facturaEncabezado);
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
                foreach (FacturaEncabezado factura in hospital.FacturaEncabezado.ToList())
                {
                    Console.Clear();
                    Console.WriteLine($"Egreso: #{index}");

                    MostrarInformacion(factura);

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
                        Console.WriteLine("No existe ningún Detalle de Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                int nuevoIdNCF;

                do
                {
                    Console.WriteLine("Escribe el ID del NCF (actualizado): ");
                    nuevoIdNCF = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.NCF.Any(
                        ncf => ncf.Id == nuevoIdNCF
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún ID del NCF");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escriba el NCF (actualizado): ");
                string NCF = Console.ReadLine();

                string nuevoDocumentoCliente;

                do
                {
                    Console.WriteLine("Escribe el documento del Cliente (actualizado): ");
                    nuevoDocumentoCliente = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Persona.Any(
                        persona => persona.Persona_Documento == nuevoDocumentoCliente
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cliente con ese documento");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                string nuevoCajeroID;

                do
                {
                    Console.WriteLine("Escribe el ID del Cajero (actualizado): ");
                    nuevoCajeroID = Console.ReadLine();

                    Console.Clear();

                    exists = hospital.Caja_Usuario.Any(
                        caj => Equals(caj.Caja_Usuario_IdCaja, nuevoCajeroID)
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningún Cajero con ese ID");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                Console.WriteLine("Escribe el Total Bruto (actualizado): ");
                Decimal nuevoTotalBruto = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Cobertura: ");
                Decimal nuevoTotalCobertura = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total ITBIS: ");
                Decimal nuevoTotalITBIS = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total Descuento: ");
                Decimal nuevoTotalDescuento = Decimal.Parse(Console.ReadLine());

                Console.WriteLine("Escribe el Total General: ");
                Decimal nuevoTotalGeneral = Decimal.Parse(Console.ReadLine());

                FacturaEncabezado nuevaFactura = hospital.FacturaEncabezado
                    .Where(
                        detalle =>
                            detalle.FacturaEncabezado_Id == idFactura
                    ).First();

                nuevaFactura.FacturaEncabezado_IdNCF = nuevoIdNCF;
                nuevaFactura.FacturaEncabezado_NCF = NCF;
                nuevaFactura.FacturaEncabezado_IdCliente = nuevoDocumentoCliente;
                nuevaFactura.FacturaEncabezado_IdCajero = nuevoCajeroID;
                nuevaFactura.FacturaEncabezado_TotalBruto = nuevoTotalBruto;
                nuevaFactura.FacturaEncabezado_TotalCobertura = nuevoTotalCobertura;
                nuevaFactura.FacturaEncabezado_TotalItbis = nuevoTotalITBIS;
                nuevaFactura.FacturaEncabezado_TotalDescuento = nuevoTotalDescuento;
                nuevaFactura.FacturaEncabezado_TotalGeneral = nuevoTotalGeneral;
                nuevaFactura.FacturaEncabezado_FechaCreacion = DateTime.Now;
                nuevaFactura.FacturaEncabezado_IdUsuarioCreador = Program.loggerUserID;
                nuevaFactura.FacturaEncabezado_Vigencia = true;

                hospital.SaveChanges();

                Logger.Info($"Se ha actualizado la factura ID:{nuevaFactura.FacturaEncabezado_Id}.");
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

                    exists = hospital.FacturaEncabezado.Any(
                        factura => factura.FacturaEncabezado_Id == idFactura
                        );

                    if (!exists)
                    {
                        Console.WriteLine("No existe ningúna Factura con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.FacturaEncabezado.Where(
                        factura => factura.FacturaEncabezado_Id == idFactura
                    ).First().FacturaEncabezado_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Factura con el ID: {idFactura}.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }
        }
    }
}
