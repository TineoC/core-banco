using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class ReciboIngreso
    {
        public int ReciboIngresoId { get; set; }
        public string? ReciboIngresoIdPaciente { get; set; }
        public string? ReciboIngresoIdCajero { get; set; }
        public decimal? ReciboIngresoMonto { get; set; }
        public int? ReciboIngresoIdCuentaDetalle { get; set; }
        public DateTime? ReciboIngresoFechaCreacion { get; set; }
        public int? ReciboIngresoIdUsuarioCreador { get; set; }
        public bool? ReciboIngresoVigencia { get; set; }

        public virtual Persona? ReciboIngresoIdCajeroNavigation { get; set; }
        public virtual FacturaEncabezado? ReciboIngresoIdCuentaDetalleNavigation { get; set; }
        public virtual Persona? ReciboIngresoIdPacienteNavigation { get; set; }
    }
}
