using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Cuenta
    {
        public Cuenta()
        {
            CuentaFacturas = new HashSet<CuentaFactura>();
            Pagos = new HashSet<Pago>();
        }

        public int CuentaId { get; set; }
        public string? CuentaIdPersona { get; set; }
        public decimal? CuentaBalance { get; set; }
        public DateTime? CuentaFechaCreacion { get; set; }
        public int? CuentaIdUsuarioCreador { get; set; }
        public bool? CuentaVigencia { get; set; }

        public virtual Persona? CuentaIdPersonaNavigation { get; set; }
        public virtual ICollection<CuentaFactura> CuentaFacturas { get; set; }
        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
