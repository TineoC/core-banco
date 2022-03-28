using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Cuenta
    {
        public int Id { get; set; }
        public int PersonaId { get; set; }
        public decimal Balance { get; set; }
        public int? UsuarioCreadorId { get; set; }
        public bool? Vigencia { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
