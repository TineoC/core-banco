using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class TipoDocumento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
