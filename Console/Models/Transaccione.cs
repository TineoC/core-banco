using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Transaccione
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public int? TipoTransaccion { get; set; }
        public int? DebitoCredito { get; set; }
        public string? Comentario { get; set; }
        public string? NumeroCuenta { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
