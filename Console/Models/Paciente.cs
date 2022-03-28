using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Paciente
    {
        public int TipoDocumento { get; set; }
        public string Documento { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public int TipoPersona { get; set; }
        public string CorreoElectronico { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public int? AseguradoraId { get; set; }
        public int? UsuarioCreadorId { get; set; }
        public bool? Vigencia { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
