using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class TipoPagoEntities
    {
        public int TipoPagoId { get; set; }

        public string TipoPagoDescripcion { get; set; }
        public DateTime? TipoPagoFechaCreacion { get; set; }
        public int? TipoPagoIdUsuarioCreador { get; set; }
        public bool? TipoPagoVigencia { get; set; }
        public int EntidadId = 18;
    }
}
