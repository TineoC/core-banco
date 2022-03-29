using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class FacturaDetalle
    {
        public int FacturaDetalleId { get; set; }
        public int? FacturaDetalleIdFactura { get; set; }
        public int? FacturaDetallePlanDeTratamiento { get; set; }
        public decimal? FacturaDetalleMontoBruto { get; set; }
        public decimal? FacturaDetalleMontoCobertura { get; set; }
        public decimal? FacturaDetalleMontoItbis { get; set; }
        public decimal? FacturaDetalleMontoDescuento { get; set; }
        public decimal? FacturaDetalleMontoTotal { get; set; }
        public DateTime? FacturaDetalleFechaCreacion { get; set; }
        public int? FacturaDetalleIdUsuarioCreador { get; set; }
        public bool? FacturaDetalleVigencia { get; set; }

        public virtual FacturaEncabezado? FacturaDetalleIdFacturaNavigation { get; set; }
        public virtual PlanDeTratamiento? FacturaDetallePlanDeTratamientoNavigation { get; set; }
    }
}
