using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class FacturaEncabezadoEntities
    {
        public int FacturaEncabezadoId { get; set; }
        public int? FacturaEncabezadoIdNcf { get; set; }
        public string FacturaEncabezadoNcf { get; set; }
        public string FacturaEncabezadoIdCliente { get; set; }
        public string FacturaEncabezadoIdCajero { get; set; }
        public decimal? FacturaEncabezadoTotalBruto { get; set; }
        public decimal? FacturaEncabezadoTotalCobertura { get; set; }
        public decimal? FacturaEncabezadoTotalItbis { get; set; }
        public decimal? FacturaEncabezadoTotalDescuento { get; set; }
        public decimal? FacturaEncabezadoTotalGeneral { get; set; }
        public DateTime? FacturaEncabezadoFechaCreacion { get; set; }
        public int? FacturaEncabezadoIdUsuarioCreador { get; set; }
        public bool? FacturaEncabezadoVigencia { get; set; }
        public int EntidadId { get; set; }
    }
}
