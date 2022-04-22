using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class AseguradoraEntities
    {
        public int AseguraodraId { get; set; }
        public string AseguradoraDescripcion { get; set; }
        public DateTime? AseguradoraFechaCreacion { get; set; }
        public int? AseguradoraIdUsuarioCreador { get; set; }
        public bool? AseguradoraVigencia { get; set; }
        public int EntidadId { get; set; }
    }
}
