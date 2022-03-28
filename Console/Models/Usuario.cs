using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Usuario
    {
        public int Id { get; set; }
        public string Nickname { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public int? PerfilId { get; set; }
        public int? UsuarioCreadorId { get; set; }
        public bool? Vigencia { get; set; }
        public DateTime CreatedAt { get; set; }
    
        public static void Crear(Usuario usuario)
        {
            using (var context = new CoreContext())
            {
                context.Usuarios.Add(usuario);
                context.SaveChanges();
            }
        }

        public static void Obtener(int usuarioID)
        {
            using (var context = new CoreContext())
            {
                Usuario? findedUser = context.Usuarios.Find(usuarioID);

                if (findedUser != null)
                {
                    Console.WriteLine($"User with id: {usuarioID}\n{findedUser}\n");
                }

                context.SaveChanges();
            }
        }

        public static void Actualizar(int usuarioID, Usuario newUserData)
        {
            using (var context = new CoreContext())
            {
                Usuario? findedUser = context.Usuarios.Find(usuarioID);

                if (findedUser != null)
                {
                    findedUser = newUserData;
                }

                context.SaveChanges();
            }
        }

        public static void Eliminar(int usuarioID)
        {
            using (var context = new CoreContext())
            {
                Usuario? findedUser = context.Usuarios.Find(usuarioID);

                if (findedUser != null)
                {
                    context.Usuarios.Remove(findedUser);
                }

                context.SaveChanges();
            }
        }
    }
}
