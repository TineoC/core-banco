using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class PlanDeTratamiento
    {
        public PlanDeTratamiento()
        {
            FacturaDetalles = new HashSet<FacturaDetalle>();
        }

        public int PlanDeTratamientoId { get; set; }
        public int? PlanDeTratamientoIdProcedimiento { get; set; }
        public int? PlanDeTratamientoNoAutorizacion { get; set; }
        public string? PlanDeTratamientoIdPaciente { get; set; }
        public string? PlanDeTratamientoIdMedico { get; set; }
        public string? PlanDeTratamientoCausa { get; set; }
        public string? PlanDeTratamientoResultado { get; set; }
        public byte[]? PlanDeTratamientoImagen { get; set; }
        public DateTime? PlanDeTratamientoFechaCreacion { get; set; }
        public int? PlanDeTratamientoIdUsuarioCreador { get; set; }
        public bool? PlanDeTratamientoVigencia { get; set; }

        public virtual Persona? PlanDeTratamientoIdMedicoNavigation { get; set; }
        public virtual Persona? PlanDeTratamientoIdPacienteNavigation { get; set; }
        public virtual ProcesoMedico? PlanDeTratamientoIdProcedimientoNavigation { get; set; }
        public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; }
    }
}
