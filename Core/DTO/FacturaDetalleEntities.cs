using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class FacturaDetalleEntities
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
        public int EntidadId = 9;
    }
}
