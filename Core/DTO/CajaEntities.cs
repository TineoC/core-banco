using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CajaEntities
    {
        public int CajaId { get; set; }
        public string CajaDescripcion { get; set; }
        public DateTime? CajaFechaCreacion { get; set; }
        public int? CajaIdUsuarioCreador { get; set; }
        public bool? CajaVigencia { get; set; }
        public int EntidadId { get; set; }
    }
}
