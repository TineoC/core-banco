using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class CuentaFactura
    {
        public int CuentaFacturaIdFactura { get; set; }
        public int CuentaFacturaIdCuenta { get; set; }
        public decimal? CuentaFacturaMonto { get; set; }
        public DateTime? CuentaFacturaFechaCreacion { get; set; }
        public int? CuentaFacturaDUsuarioCreador { get; set; }
        public bool? CuentaFacturaVigencia { get; set; }

        public virtual Cuenta CuentaFacturaIdCuentaNavigation { get; set; } = null!;
        public virtual FacturaEncabezado CuentaFacturaIdFacturaNavigation { get; set; } = null!;
    }
}
