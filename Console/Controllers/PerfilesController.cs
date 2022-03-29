using Core_Console.Models;

namespace Core_Console.Controllers
{
    public class PerfilesController
    {
        HospitalContext? dbContext = null;
        public void CrearPerfil(Perfil perfil)
        {
            dbContext = new HospitalContext();
            dbContext.Perfils.Add(perfil);
            dbContext.SaveChanges();
        }

        public void MostrarPerfiles()
        {
            dbContext = new HospitalContext();

            foreach (var perfil in dbContext.Perfils)
            {
                Console.WriteLine(perfil);
            }

            dbContext.SaveChanges();
        }

        public void MostrarPerfil(int perfilID)
        {
            dbContext = new HospitalContext();

            Perfil? findedPerfil = dbContext.Perfils.Find(perfilID);

            Console.WriteLine(findedPerfil);

            dbContext.SaveChanges();
        }

        public void ActualizarPerfil(int perfilID, Perfil newPerfil)
        {
            dbContext = new HospitalContext();
            Perfil? findedPerfil = dbContext.Perfils.Find(perfilID);

            if (findedPerfil != null) { findedPerfil = newPerfil; }
        }

        public void EliminarPerfil(int perfilID)
        {
            dbContext = new HospitalContext();
            Perfil? findedPerfil = dbContext.Perfils.Find(perfilID);

            if (findedPerfil != null) { dbContext.Perfils.Remove(findedPerfil); }

        }
    }
}