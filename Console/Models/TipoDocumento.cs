using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Personas = new HashSet<Persona>();
        }

        public int TipoDocumentoId { get; set; }
        public string? TipoDocumentoDescripcion { get; set; }
        public DateTime? TipoDocumentoFechaCreacion { get; set; }
        public int? TipoDocumentoIdUsuarioCreador { get; set; }
        public bool? TipoDocumentoVigencia { get; set; }

        public virtual ICollection<Persona> Personas { get; set; }
    }
}
