using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Aseguradora
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public int? UsuarioCreadorId { get; set; }
        public bool? Vigencia { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
