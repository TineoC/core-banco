using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class LogGeneral
    {
        public int LogGeneralId { get; set; }
        public string? LogGeneralPantalla { get; set; }
        public string? LogGeneralAccion { get; set; }
        public string? LogGeneralMensaje { get; set; }
        public DateTime? LogGeneralFecha { get; set; }
        public int? LogGeneralIdUsuario { get; set; }
        public bool? LogGeneralVigencia { get; set; }

        public virtual Usuario? LogGeneralIdUsuarioNavigation { get; set; }
    }
}
