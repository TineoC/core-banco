using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CuentaFacturaEntities
    {
        public int CuentaFacturaIdFactura { get; set; }
        public int CuentaFacturaIdCuenta { get; set; }
        public decimal? CuentaFacturaMonto { get; set; }
        public DateTime? CuentaFacturaFechaCreacion { get; set; }
        public int? CuentaFacturaDUsuarioCreador { get; set; }
        public bool? CuentaFacturaVigencia { get; set; }
        public int LastFacturaId { get; set; }
        public int LastCuentaId { get; set; }
        public int EntidadId = 7;
    }
}
