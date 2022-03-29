using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class ProcesoMedico
    {
        public ProcesoMedico()
        {
            Autorizacions = new HashSet<Autorizacion>();
            PlanDeTratamientos = new HashSet<PlanDeTratamiento>();
        }

        public int ProcesoMedicoId { get; set; }
        public int? ProcesoMedicoDescripcion { get; set; }
        public decimal? ProcesoMedicoPrecio { get; set; }
        public DateTime? ProcesoMedicoFechaCreacion { get; set; }
        public int? ProcesoMedicoIdUsuarioCreador { get; set; }
        public bool? ProcesoMedicoVigencia { get; set; }
        public int? ProcesoMedicoIdTipoProceso { get; set; }

        public virtual TipoProceso? ProcesoMedicoIdTipoProcesoNavigation { get; set; }
        public virtual ICollection<Autorizacion> Autorizacions { get; set; }
        public virtual ICollection<PlanDeTratamiento> PlanDeTratamientos { get; set; }
    }
}
