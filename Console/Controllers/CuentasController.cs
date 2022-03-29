using Core_Console.Models;

namespace Core_Console.Controllers
{
    public class CuentasController
    {
        HospitalContext? dbContext = null;
        public void CrearCuenta(Cuenta cuenta)
        {
            dbContext = new HospitalContext();
            dbContext.Cuentas.Add(cuenta);
            dbContext.SaveChanges();
        }

        public void MostrarCuentas()
        {
            dbContext = new HospitalContext();

            foreach (var cuenta in dbContext.Cuentas)
            {
                Console.WriteLine(cuenta);
            }

            dbContext.SaveChanges();
        }

        public void MostrarCuenta(int cuentaID)
        {
            dbContext = new HospitalContext();

            Cuenta? findedCuenta = dbContext.Cuentas.Find(cuentaID);

            Console.WriteLine(findedCuenta);

            dbContext.SaveChanges();
        }

        public void ActualizarCuenta(int cuentaID, Cuenta newCuenta)
        {
            dbContext = new HospitalContext();
            Cuenta? findedCuenta = dbContext.Cuentas.Find(cuentaID);

            if (findedCuenta != null) { findedCuenta = newCuenta; }
        }

        public void EliminarCuenta(int cuentaID)
        {
            dbContext = new HospitalContext();
            Cuenta? findedCuenta = dbContext.Cuentas.Find(cuentaID);

            if (findedCuenta != null) { dbContext.Cuentas.Remove(findedCuenta); }

        }
    }
}