using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    class PerfilEntities
    {
        public int PerfilId { get; set; }
        public string PerfilDescripcion { get; set; }
        public DateTime? PerfilFechaCreacion { get; set; }
        public int? PerfilIdUsuarioCreador { get; set; }
        public bool? PerfilVigencia { get; set; }
        public int EntidadId = 12;
    }
}
