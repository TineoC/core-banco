using Core_Console.Models;

namespace Core_Console.Controllers
{
    public class AutorizacionesController
    {
        HospitalContext? dbContext = null;
        public void CrearAutorizaciones(Autorizacion autorizacion)
        {
            dbContext = new HospitalContext();
            dbContext.Autorizacions.Add(autorizacion);
            dbContext.SaveChanges();
        }

        public void MostrarAutorizaciones()
        {
            dbContext = new HospitalContext();

            foreach (var autorizacion in dbContext.Autorizacions)
            {
                Console.WriteLine(autorizacion);
            }

            dbContext.SaveChanges();
        }

        public void MostrarAutorizacion(int autorizacionID)
        {
            dbContext = new HospitalContext();

            Autorizacion? findedAutorizacion = dbContext.Autorizacions.Find(autorizacionID);

            Console.WriteLine(findedAutorizacion);

            dbContext.SaveChanges();
        }

        public void ActualizarAutorizacion(int autorizacionID, Autorizacion newAutorizacion)
        {
            dbContext = new HospitalContext();
            Autorizacion? findedAutorizacion = dbContext.Autorizacions.Find(autorizacionID);

            if (findedAutorizacion != null) { findedAutorizacion = newAutorizacion; }
        }

        public void EliminarAutorizacion(int autorizacionID)
        {
            dbContext = new HospitalContext();
            Autorizacion? findedAutorizacion = dbContext.Autorizacions.Find(autorizacionID);

            if (findedAutorizacion != null) { dbContext.Autorizacions.Remove(findedAutorizacion); }

        }
    }
}