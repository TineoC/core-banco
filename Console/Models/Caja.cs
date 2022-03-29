using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Caja
    {
        public Caja()
        {
            AperturaYcierreDeCajas = new HashSet<AperturaYcierreDeCaja>();
            CajaUsuarios = new HashSet<CajaUsuario>();
        }

        public int CajaId { get; set; }
        public string? CajaDescripcion { get; set; }
        public DateTime? CajaFechaCreacion { get; set; }
        public int? CajaIdUsuarioCreador { get; set; }
        public bool? CajaVigencia { get; set; }

        public virtual ICollection<AperturaYcierreDeCaja> AperturaYcierreDeCajas { get; set; }
        public virtual ICollection<CajaUsuario> CajaUsuarios { get; set; }
    }
}
