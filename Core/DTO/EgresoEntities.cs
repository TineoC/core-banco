using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class EgresoEntities
    {
        public int EgresoId { get; set; }
        public string EgresoPagadoA { get; set; }
        public string EgresoCedula { get; set; }
        public decimal? EgresoMonto { get; set; }
        public string EgresoConcepto { get; set; }
        public string EgresoPreparado { get; set; }
        public string EgrespAprobado { get; set; }
        public string EgresoRecibido { get; set; }
        public DateTime? EgresoFechaCreacion { get; set; }
        public int? EgresoIdUsuarioCreador { get; set; }
        public bool? EgresoVigencia { get; set; }
        public int EntidadId = 8;
    }
}
