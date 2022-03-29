using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class CajaUsuario
    {
        public int CajaUsuarioIdUsuario { get; set; }
        public int CajaUsuarioIdCaja { get; set; }
        public DateTime? CajaUsuarioIdUsuarioFechaCreacion { get; set; }
        public int? CajaUsuarioIdUsuarioIdUsuarioCreador { get; set; }
        public bool? CajaUsuarioIdUsuarioVigencia { get; set; }

        public virtual Caja CajaUsuarioIdCajaNavigation { get; set; } = null!;
        public virtual Usuario CajaUsuarioIdUsuarioNavigation { get; set; } = null!;
    }
}
