using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class TipoDocumentoEntities
    {
        public int TipoDocumentoId { get; set; }
        public string TipoDocumentoDescripcion { get; set; }
        public DateTime? TipoDocumentoFechaCreacion { get; set; }
        public int? TipoDocumentoIdUsuarioCreador { get; set; }
        public bool? TipoDocumentoVigencia { get; set; }
        public int EntidadId = 17;
    }
}
