using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Autorizacion
    {
        public int AutorizacionId { get; set; }
        public int? AutorizacionIdAseguradora { get; set; }
        public int? AutorizacionIdProcedimiento { get; set; }
        public DateTime? AutorizacionFechaCreacion { get; set; }
        public int? AutorizacionIdUsuarioCreador { get; set; }
        public bool? AutorizacionVigencia { get; set; }
        public decimal? AutorizacionPrecio { get; set; }
        public decimal? AutorizacionCobertura { get; set; }
        public decimal? AutorizacionDiferencia { get; set; }
        public int? AutorizacionNoAutorizacion { get; set; }

        public virtual Aseguradora? AutorizacionIdAseguradoraNavigation { get; set; }
        public virtual ProcesoMedico? AutorizacionIdProcedimientoNavigation { get; set; }
    }
}
