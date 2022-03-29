using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class TipoPersona
    {
        public TipoPersona()
        {
            Personas = new HashSet<Persona>();
        }

        public int TipoPersonaId { get; set; }
        public string? TipoPersonaDescripcion { get; set; }
        public DateTime? TipoPersonaFechaCreacion { get; set; }
        public int? TipoPersonaIdUsuarioCreador { get; set; }
        public bool? TipoPersonaVigencia { get; set; }

        public virtual ICollection<Persona> Personas { get; set; }
    }
}
