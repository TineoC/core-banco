using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Aseguradora
    {
        public Aseguradora()
        {
            Autorizacions = new HashSet<Autorizacion>();
            Personas = new HashSet<Persona>();
        }

        public int AseguraodraId { get; set; }
        public string? AseguradoraDescripcion { get; set; }
        public DateTime? AseguradoraFechaCreacion { get; set; }
        public int? AseguradoraIdUsuarioCreador { get; set; }
        public bool? AseguradoraVigencia { get; set; }

        public virtual ICollection<Autorizacion> Autorizacions { get; set; }
        public virtual ICollection<Persona> Personas { get; set; }
    }
}
