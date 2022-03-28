using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Procedimiento
    {
        public int Id { get; set; }
        public string Descripcion { get; set; } = null!;
        public decimal Precio { get; set; }
        public int? UsuarioCreadorId { get; set; }
        public bool? Vigencia { get; set; }
        public int TipoProcesoId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
