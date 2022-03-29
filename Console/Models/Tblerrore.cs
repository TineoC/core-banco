using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Tblerrore
    {
        public int TblerroresId { get; set; }
        public string? TblerroresPantalla { get; set; }
        public string? TblerroresMetodo { get; set; }
        public string? TblerroresMensaje { get; set; }
        public int? TblerroresIdUsuario { get; set; }
        public bool? TblerroresVigencia { get; set; }
    }
}
