using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class UsuarioEntities
    {
        public int UsuarioId { get; set; }
        public string UsuarioNickname { get; set; }
        public string UsuarioContraseña { get; set; }
        public int? UsuarioIdPerfil { get; set; }
        public string IdPersona { get; set; }
        public DateTime? UsuarioFechaCreacion { get; set; }
        public int? UsuarioIdUsuarioCreador { get; set; }
        public bool? UsuarioVigencia { get; set; }
        public int EntidadId = 21;
    }
}
