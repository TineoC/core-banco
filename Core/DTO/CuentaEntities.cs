using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CuentaEntities
    {
        public int CuentaId { get; set; }
        public string CuentaIdPersona { get; set; }
        public decimal? CuentaBalance { get; set; }
        public DateTime? CuentaFechaCreacion { get; set; }
        public int? CuentaIdUsuarioCreador { get; set; }
        public bool? CuentaVigencia { get; set; }
        public int EntidadId = 6;
    }
}
