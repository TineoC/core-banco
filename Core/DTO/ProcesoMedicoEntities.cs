using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class ProcesoMedicoEntities
    {
        public int ProcesoMedicoId { get; set; }
        public string ProcesoMedicoDescripcion { get; set; }
        public decimal? ProcesoMedicoPrecio { get; set; }
        public DateTime? ProcesoMedicoFechaCreacion { get; set; }
        public int? ProcesoMedicoIdUsuarioCreador { get; set; }
        public bool? ProcesoMedicoVigencia { get; set; }
        public int? ProcesoMedicoIdTipoProceso { get; set; }
        public int EntidadId { get; set; }
    }
}
