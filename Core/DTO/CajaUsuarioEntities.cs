using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class CajaUsuarioEntities
    {
        public int CajaUsuarioIdUsuario { get; set; }
        public int CajaUsuarioIdCaja { get; set; }
        public DateTime? CajaUsuarioIdUsuarioFechaCreacion { get; set; }
        public int? CajaUsuarioIdUsuarioIdUsuarioCreador { get; set; }
        public bool? CajaUsuarioIdUsuarioVigencia { get; set; }
        public int LastCajaUsuarioIdUsuario { get; set; }
        public int LastCajaUsuarioIdCaja { get; set; }
        public int EntidadId { get; set; }
    }
}
