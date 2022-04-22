using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class AutorizacionEntities
    {
        public int AutorizacionId { get; set; }
        public int? AutorizacionIdAseguradora { get; set; }
        public int? AutorizacionIdProcedimiento { get; set; }
        public DateTime? AutorizacionFechaCreacion { get; set; }
        public int? AutorizacionIdUsuarioCreador { get; set; }
        public bool? AutorizacionVigencia { get; set; }
        public decimal? AutorizacionPrecio { get; set; }
        public decimal? AutorizacionCobertura { get; set; }
        public decimal? AutorizacionDiferencia { get; set; }
        public int? AutorizacionNoAutorizacion { get; set; }
        public int EntidadId = 3;
    }
}
