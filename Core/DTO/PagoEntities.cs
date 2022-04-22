using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class PagoEntities
    {
        public int PagoId { get; set; }
        public string PagoIdPersona { get; set; }
        public int? PagoIdCuenta { get; set; }
        public string PagoReferencia { get; set; }
        public decimal? PagoMonto { get; set; }
        public int? PagoTipoPago { get; set; }
        public DateTime? PagoFechaCreacion { get; set; }
        public int? PagoIdUsuarioCreador { get; set; }
        public bool? PagoVigencia { get; set; }
        public int? CajaId { get; set; }
        public int EntidadId { get; set; }
    }
}
