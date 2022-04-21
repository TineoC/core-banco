using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class TipoPersonaEntities
    {
        public int TipoPersonaId { get; set; }
        public string TipoPersonaDescripcion { get; set; }
        public DateTime? TipoPersonaFechaCreacion { get; set; }
        public int? TipoPersonaIdUsuarioCreador { get; set; }
        public bool? TipoPersonaVigencia { get; set; }
        public int EntidadId = 19;
    }
}
