using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Pago
    {
        public int PagoId { get; set; }
        public string? PagoIdPersona { get; set; }
        public int? PagoIdCuenta { get; set; }
        public string? PagoReferencia { get; set; }
        public decimal? PagoMonto { get; set; }
        public int? PagoTipoPago { get; set; }
        public DateTime? PagoFechaCreacion { get; set; }
        public int? PagoIdUsuarioCreador { get; set; }
        public bool? PagoVigencia { get; set; }

        public virtual Cuenta? PagoIdCuentaNavigation { get; set; }
        public virtual Persona? PagoIdPersonaNavigation { get; set; }
        public virtual Usuario? PagoIdUsuarioCreadorNavigation { get; set; }
        public virtual TipoPago? PagoTipoPagoNavigation { get; set; }
    }
}
