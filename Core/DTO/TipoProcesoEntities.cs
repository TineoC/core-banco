using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class TipoProcesoEntities
    {
        public int TipoProcesoId { get; set; }
        public string TipoProcesoDescripcion { get; set; }
        public DateTime? TipoProcesoFechaCreacion { get; set; }
        public int? TipoProcesoIdUsuarioCreador { get; set; }
        public bool? TipoProcesoVigencia { get; set; }
        public int EntidadId { get; set; }
    }
}
