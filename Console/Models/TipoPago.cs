using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class TipoPago
    {
        public TipoPago()
        {
            Pagos = new HashSet<Pago>();
        }

        public int TipoPagoId { get; set; }
        public string? TipoPagoDescripcion { get; set; }
        public DateTime? TipoPagoFechaCreacion { get; set; }
        public int? TipoPagoIdUsuarioCreador { get; set; }
        public bool? TipoPagoVigencia { get; set; }

        public virtual ICollection<Pago> Pagos { get; set; }
    }
}
