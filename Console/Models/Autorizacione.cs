using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Autorizacione
    {
        public int Id { get; set; }
        public int? Numero { get; set; }
        public int? AseguradoraId { get; set; }
        public int? ProcedimientoId { get; set; }
        public int? UsuarioCreadorId { get; set; }
        public decimal? Precio { get; set; }
        public decimal? Cobertura { get; set; }
        public decimal? Diferencia { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? Vigencia { get; set; }
    }
}
