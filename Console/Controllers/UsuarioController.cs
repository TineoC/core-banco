using Core_Console.Models;

namespace Core_Console.Controllers
{
    public class UsuarioController
    {
        HospitalContext? dbContext = null;
        public void CrearUsuario(Usuario usuario)
        {
            dbContext = new HospitalContext();
            dbContext.Usuarios.Add(usuario);
            dbContext.SaveChanges();
        }

        public void MostrarUsuarios()
        {
            dbContext = new HospitalContext();
            
            foreach(var usuario in dbContext.Usuarios)
            {
                Console.WriteLine(usuario);
            }

            dbContext.SaveChanges();
        }

        public void MostrarUsuario(int usuarioID)
        {
            dbContext = new HospitalContext();

            Usuario? findedUsuario = dbContext.Usuarios.Find(usuarioID);

            Console.WriteLine(findedUsuario);

            dbContext.SaveChanges();
        }

        public void Actualizar(int usuarioID, Usuario newUser)
        {
            dbContext = new HospitalContext();
            Usuario? findedUser = dbContext.Usuarios.Find(usuarioID);

            if (findedUser != null) { findedUser = newUser; }
        }

        public void Eliminar(int usuarioID)
        {
            dbContext = new HospitalContext();
            Usuario? findedUser = dbContext.Usuarios.Find(usuarioID);

            if (findedUser != null) { dbContext.Usuarios.Remove(findedUser); }
            
        }
    }
}