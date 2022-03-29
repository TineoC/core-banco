using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Perfil
    {
        public Perfil()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int PerfilId { get; set; }
        public string? PerfilDescripcion { get; set; }
        public DateTime? PerfilFechaCreacion { get; set; }
        public int? PerfilIdUsuarioCreador { get; set; }
        public bool? PerfilVigencia { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
