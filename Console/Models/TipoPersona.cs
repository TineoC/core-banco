using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class TipoPersona
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public int? UsuarioCreadorId { get; set; }
        public bool? Vigencia { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
