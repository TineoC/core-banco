using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    class TipoNCFEEntitites
    {
        public int TipoNcfId { get; set; }
        public string TipoNcfDescripcion { get; set; }
        public int? TipoNcfIdUsuario { get; set; }
        public DateTime? TipoNcfFechaCreado { get; set; }
        public bool TipoNcfEstado { get; set; }
        public int? TipoNcfIdSucursal { get; set; }
        public int EntidadId = 22;
    }
}
