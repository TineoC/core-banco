using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class TipoProceso
    {
        public TipoProceso()
        {
            ProcesoMedicos = new HashSet<ProcesoMedico>();
        }

        public int TipoProcesoId { get; set; }
        public string? TipoProcesoDescripcion { get; set; }
        public DateTime? TipoProcesoFechaCreacion { get; set; }
        public int? TipoProcesoIdUsuarioCreador { get; set; }
        public bool? TipoProcesoVigencia { get; set; }

        public virtual ICollection<ProcesoMedico> ProcesoMedicos { get; set; }
    }
}
