using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class ReciboIngresoEntities
    {
        public int ReciboIngresoId { get; set; }
        public string ReciboIngresoIdPaciente { get; set; }
        public string ReciboIngresoIdCajero { get; set; }
        public decimal? ReciboIngresoMonto { get; set; }
        public int? ReciboIngresoIdCuentaDetalle { get; set; }
        public DateTime? ReciboIngresoFechaCreacion { get; set; }
        public int? ReciboIngresoIdUsuarioCreador { get; set; }
        public bool? ReciboIngresoVigencia { get; set; }
        public int EntidadId = 16;
    }
}
