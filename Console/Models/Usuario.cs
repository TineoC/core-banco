using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            AperturaYcierreDeCajas = new HashSet<AperturaYcierreDeCaja>();
            CajaUsuarios = new HashSet<CajaUsuario>();
            Egresos = new HashSet<Egreso>();
            LogGenerals = new HashSet<LogGeneral>();
            Pagos = new HashSet<Pago>();
        }

        public int UsuarioId { get; set; }
        public string? UsuarioNickname { get; set; }
        public string? UsuarioContraseña { get; set; }
        public int? UsuarioIdPerfil { get; set; }
        public string? IdPersona { get; set; }
        public DateTime? UsuarioFechaCreacion { get; set; }
        public int? UsuarioIdUsuarioCreador { get; set; }
        public bool? UsuarioVigencia { get; set; }

        public virtual Persona? IdPersonaNavigation { get; set; }
        public virtual Perfil? UsuarioIdPerfilNavigation { get; set; }
        public virtual ICollection<AperturaYcierreDeCaja> AperturaYcierreDeCajas { get; set; }
        public virtual ICollection<CajaUsuario> CajaUsuarios { get; set; }
        public virtual ICollection<Egreso> Egresos { get; set; }
        public virtual ICollection<LogGeneral> LogGenerals { get; set; }
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
