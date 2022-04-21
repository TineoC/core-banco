using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Controllers
{
    internal class AperturaYCierreDeCajaController
    {
        static hospitalEntities hospital = new hospitalEntities();

        public static void MostrarInformacion(AperturaYCierreDeCaja caja)
        {
            Console.WriteLine($"ID: {caja.AperturaYCierreDeCaja_Id}");
            Console.WriteLine($"ID Caja: {caja.AperturaYCierreDeCaja_IdCaja}");
            Console.WriteLine($"($2000) Dos Mil Pesos: {caja.AperturaYCierreDeCaja_DosMIlPesos}");
            Console.WriteLine($"($1000) Mil Pesos: {caja.AperturaYCierreDeCaja_MIlPesos}");
            Console.WriteLine($"($500) Quinientos Pesos: {caja.AperturaYCierreDeCaja_QuinientosPeso}");
            Console.WriteLine($"($200) Doscientos Pesos: {caja.AperturaYCierreDeCaja_DoscientosPesos}");
            Console.WriteLine($"($100) Cien Pesos: {caja.AperturaYCierreDeCaja_CienPesos}");
            Console.WriteLine($"($50) Cincuenta Pesos: {caja.AperturaYCierreDeCaja_CincuentaPesos}");
            Console.WriteLine($"($25) Venticinco Pesos: {caja.AperturaYCierreDeCaja_25Pesos}");
            Console.WriteLine($"($10) Diez Pesos: {caja.AperturaYCierreDeCaja_10Pesos}");
            Console.WriteLine($"($5) Cinco Pesos: {caja.AperturaYCierreDeCaja_5Peso}");
            Console.WriteLine($"($1) Un Peso: {caja.AperturaYCierreDeCaja_1Peso}");
            Console.WriteLine($"Total Efectivo: {caja.AperturaYCierreDeCaja_TotalEfectivo}");
            Console.WriteLine($"Total Crédito: {caja.AperturaYCierreDeCaja_TotalCredito}");
            Console.WriteLine($"Total Tarjeta de Crédito: {caja.AperturaYCierreDeCaja_TotalTarjeta}");
            Console.WriteLine($"Total Transferencia: {caja.AperturaYCierreDeCaja_Transferencia}");
            Console.WriteLine($"Depósito: {caja.AperturaYCierreDeCaja_Deposito}");
            Console.WriteLine($"Total Cheques: {caja.AperturaYCierreDeCaja_TotalCheques}");
            Console.WriteLine($"Total General: {caja.AperturaYCierreDeCaja_TotalGeneral}");
            Console.WriteLine($"Apertura O Cierre: {caja.AperturaYCierreDeCaja_AperturaOCierre}");
            Console.WriteLine($"Fecha: {caja.AperturaYCierreDeCaja_Fecha}");
            Console.WriteLine($"ID Usuario Creador: {caja.AperturaYCierreDeCaja_IdUsuarioCreador}");
            Console.WriteLine($"Vigencia: {caja.AperturaYCierreDeCaja_Vigencia}");
        }

        public static void Crear()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            Console.Clear();

            try
            {
                Console.Write("Escribe el ID de Caja: ");
                int idCaja = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Dos Mil Pesos: ");
                int dosMilPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Mil Pesos: ");
                int milPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Quinientos Pesos: ");
                int quinientosPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Doscientos Pesos: ");
                int doscientosPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Cien Pesos: ");
                int cienPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Cincuenta Pesos: ");
                int cincuentaPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Venticinco Pesos: ");
                int venticincoPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Diez Pesos: ");
                int diezPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Cinco Pesos: ");
                int cincoPesos = Int32.Parse(Console.ReadLine());

                Console.Write("Cantidad Un Peso: ");
                int unPeso = Int32.Parse(Console.ReadLine());

                Console.Write("Total Efectivo: ");
                int totalEfectivo = Int32.Parse(Console.ReadLine());

                Console.Write("Total Crédito: ");
                Decimal totalCredito = Decimal.Parse(Console.ReadLine());

                Console.Write("Total Tarjeta: ");
                Decimal totalTarjeta = Decimal.Parse(Console.ReadLine());

                Console.Write("Total Transferencia: ");
                Decimal totalTransferencia = Decimal.Parse(Console.ReadLine());

                Console.Write("Total Depósito: ");
                Decimal totalDeposito = Decimal.Parse(Console.ReadLine());

                Console.Write("Total Cheques: ");
                Decimal totalCheques = Decimal.Parse(Console.ReadLine());

                Console.Write("Total General: ");
                Decimal totalGeneral = Decimal.Parse(Console.ReadLine());

                hospital.AperturaYCierreDeCaja.Add(new AperturaYCierreDeCaja()
                {
                    AperturaYCierreDeCaja_IdCaja = idCaja,
                    AperturaYCierreDeCaja_DosMIlPesos = dosMilPesos,
                    AperturaYCierreDeCaja_MIlPesos = milPesos,
                    AperturaYCierreDeCaja_QuinientosPeso = quinientosPesos,
                    AperturaYCierreDeCaja_DoscientosPesos = doscientosPesos,
                    AperturaYCierreDeCaja_CienPesos = cienPesos,
                    AperturaYCierreDeCaja_CincuentaPesos = cincuentaPesos,
                    AperturaYCierreDeCaja_25Pesos = venticincoPesos,
                    AperturaYCierreDeCaja_10Pesos = diezPesos,
                    AperturaYCierreDeCaja_5Peso = cincoPesos,
                    AperturaYCierreDeCaja_1Peso = unPeso,
                    AperturaYCierreDeCaja_TotalEfectivo = totalEfectivo,
                    AperturaYCierreDeCaja_TotalCredito = totalCredito,
                    AperturaYCierreDeCaja_TotalTarjeta = totalTarjeta,
                    AperturaYCierreDeCaja_Transferencia = totalTransferencia,
                    AperturaYCierreDeCaja_Deposito = totalDeposito,
                    AperturaYCierreDeCaja_TotalCheques = totalCheques,
                    AperturaYCierreDeCaja_TotalGeneral = totalGeneral,
                    AperturaYCierreDeCaja_AperturaOCierre = true,
                    AperturaYCierreDeCaja_Fecha = DateTime.Now,
                    AperturaYCierreDeCaja_IdUsuarioCreador = Program.loggerUserID,
                    AperturaYCierreDeCaja_Vigencia = true
                });

                Logger.Info($"Se ha creado la Apertura o Cierre de Caja correctamente");

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
            int id;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el id de la Apertura o Cierre de Caja a mostrar: ");

                id = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.AperturaYCierreDeCaja.Any(aper => aper.AperturaYCierreDeCaja_Id == id);

                if (!exists)
                {
                    Console.WriteLine("No existen Aperturas o Cierres de Caja con ese id");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Clear();

            AperturaYCierreDeCaja aperturaYCierreDeCaja = hospital.AperturaYCierreDeCaja
                            .Where(
                                aper => aper.AperturaYCierreDeCaja_Id == id
                            )
                            .FirstOrDefault();

            MostrarInformacion(aperturaYCierreDeCaja);
        }
        public static void MostrarTodos()
        {
            int index = 1;
            foreach (AperturaYCierreDeCaja aperturaYCierreDeCaja in hospital.AperturaYCierreDeCaja.ToList())
            {
                Console.Clear();
                Console.WriteLine($"Apertura o Cierre de Caja: {index}");

                MostrarInformacion(aperturaYCierreDeCaja);

                index++;
            }
        }
        public static void Actualizar()
        {
            bool exists = false;
            int id;
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            do
            {
                Console.Write("Escribe el id de la Apertura o Cierre de Caja a mostrar: ");

                id = Int32.Parse(Console.ReadLine());

                Console.Clear();

                exists = hospital.AperturaYCierreDeCaja.Any(aper => aper.AperturaYCierreDeCaja_Id == id);

                if (!exists)
                {
                    Console.WriteLine("No existen Aperturas o Cierres de Caja con ese id");

                    Console.Write("Press any key to continue...");
                    Console.ReadKey();
                }
            } while (!exists);

            Console.Write("Escribe el ID de Caja: (actualizado)");
            int idCaja = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Dos Mil Pesos: (actualizado)");
            int dosMilPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Mil Pesos: (actualizado)");
            int milPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Quinientos Pesos: (actualizado)");
            int quinientosPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Doscientos Pesos: (actualizado)");
            int doscientosPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Cien Pesos: (actualizado) ");
            int cienPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Cincuenta Pesos: (actualizado)");
            int cincuentaPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Venticinco Pesos: (actualizado)");
            int venticincoPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Diez Pesos: (actualizado)");
            int diezPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Cinco Pesos: (actualizado)");
            int cincoPesos = Int32.Parse(Console.ReadLine());

            Console.Write("Cantidad Un Peso: (actualizado)");
            int unPeso = Int32.Parse(Console.ReadLine());

            Console.Write("Total Efectivo: (actualizado)");
            int totalEfectivo = Int32.Parse(Console.ReadLine());

            Console.Write("Total Crédito: (actualizado)");
            Decimal totalCredito = Decimal.Parse(Console.ReadLine());

            Console.Write("Total Tarjeta: (actualizado)");
            Decimal totalTarjeta = Decimal.Parse(Console.ReadLine());

            Console.Write("Total Transferencia: (actualizado)");
            Decimal totalTransferencia = Decimal.Parse(Console.ReadLine());

            Console.Write("Total Depósito: (actualizado)");
            Decimal totalDeposito = Decimal.Parse(Console.ReadLine());

            Console.Write("Total Cheques: (actualizado)");
            Decimal totalCheques = Decimal.Parse(Console.ReadLine());

            Console.Write("Total General: (actualizado)");
            Decimal totalGeneral = Decimal.Parse(Console.ReadLine());

            AperturaYCierreDeCaja aperturaYCierreDeCaja = hospital.AperturaYCierreDeCaja
                .Where(
                    aper => aper.AperturaYCierreDeCaja_Id == id
                ).First();

                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_IdCaja = idCaja;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_DosMIlPesos = dosMilPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_MIlPesos = milPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_QuinientosPeso = quinientosPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_DoscientosPesos = doscientosPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_CienPesos = cienPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_CincuentaPesos = cincuentaPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_25Pesos = venticincoPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_10Pesos = diezPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_5Peso = cincoPesos;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_1Peso = unPeso;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_TotalEfectivo = totalEfectivo;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_TotalCredito = totalCredito;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_TotalTarjeta = totalTarjeta;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_Transferencia = totalTransferencia;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_Deposito = totalDeposito;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_TotalCheques = totalCheques;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_TotalGeneral = totalGeneral;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_AperturaOCierre = true;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_Fecha = DateTime.Now;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_IdUsuarioCreador = Program.loggerUserID;
                    aperturaYCierreDeCaja.AperturaYCierreDeCaja_Vigencia = true;

            Logger.Info($"Se ha actualizado la Apertura o Cierre de Caja correctamente.");

            hospital.SaveChanges();
        }
        public static void Eliminar()
        {
            var Logger = NLog.LogManager.GetCurrentClassLogger();

            try
            {
                bool exists = false;
                int id;

                do
                {
                    Console.Write("Escribe el id de la Apertura o Cierre de Caja a mostrar: ");

                    id = Int32.Parse(Console.ReadLine());

                    Console.Clear();

                    exists = hospital.AperturaYCierreDeCaja.Any(aper => aper.AperturaYCierreDeCaja_Id == id);

                    if (!exists)
                    {
                        Console.WriteLine("No existen Aperturas o Cierres de Caja con ese id");

                        Console.Write("Press any key to continue...");
                        Console.ReadKey();
                    }
                } while (!exists);

                hospital.AperturaYCierreDeCaja.Where(
                    aper => aper.AperturaYCierreDeCaja_Id == id
                    ).First().AperturaYCierreDeCaja_Vigencia = false;

                hospital.SaveChanges();

                Logger.Info($"Se ha eliminado la Apertura o Cierre de Caja con ID: {id} correctamente.");
            }

            catch (Exception e)
            {
                Logger.Error(e.Message, "Ha ocurrido un error inesperado");
                throw;
            }           
        }
    }
}
