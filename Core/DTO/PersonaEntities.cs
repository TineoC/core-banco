using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class PersonaEntities
    {
        [Column("Persona_TipoDocumento")]
        public int? PersonaTipoDocumento { get; set; }
        [Column("Persona_Documento")]
        public string PersonaDocumento { get; set; }
        [Column("Persona_Nombre")]
        public string PersonaNombre { get; set; }
        [Column("Persona_Apellido")]
        public string PersonaApellido { get; set; }
        [Column("Persona_TipoPersona")]
        public int? PersonaTipoPersona { get; set; }
        [Column("Persona_CorreoElectronico")]
        public string PersonaCorreoElectronico { get; set; }
        [Column("Persona_Telefono")]
        public string PersonaTelefono { get; set; }
        [Column("Persona_Direccion")]
        public string PersonaDireccion { get; set; }
        [Column("Persona_IdAseguradora")]
        public int? PersonaIdAseguradora { get; set; }
        [Column("Persona_FechaCreacion", TypeName = "datetime")]
        public DateTime? PersonaFechaCreacion { get; set; }
        [Column("Persona_IdUsuarioCreador")]
        public int? PersonaIdUsuarioCreador { get; set; }
        [Column("Persona_Vigencia")]
        public bool? PersonaVigencia { get; set; }
        public int EntidadId { get; set; }

        //public int? PersonaTipoDocumento { get; set; }
        //public string PersonaDocumento { get; set; }
        //public string PersonaNombre { get; set; }
        //public string PersonaApellido { get; set; }
        //public int? PersonaTipoPersona { get; set; }
        //public string PersonaCorreoElectronico { get; set; }
        //public string PersonaTelefono { get; set; }
        //public string PersonaDireccion { get; set; }
        //public int? PersonaIdAseguradora { get; set; }
        //public DateTime? PersonaFechaCreacion { get; set; }
        //public int? PersonaIdUsuarioCreador { get; set; }
        //public bool? PersonaVigencia { get; set; }
        //public int EntidadId = 13;
    }
}
