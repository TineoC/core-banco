using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTO
{
    public class PlanTratamientoVigenciaEntities
    {
        public int PlanDeTratamientoId { get; set; }
        public int? PlanDeTratamientoIdProcedimiento { get; set; }
        public int? PlanDeTratamientoNoAutorizacion { get; set; }
        public string PlanDeTratamientoIdPaciente { get; set; }
        public string PlanDeTratamientoIdMedico { get; set; }
        public string PlanDeTratamientoCausa { get; set; }
        public string PlanDeTratamientoResultado { get; set; }
        public byte[] PlanDeTratamientoImagen { get; set; }
        public DateTime? PlanDeTratamientoFechaCreacion { get; set; }
        public int? PlanDeTratamientoIdUsuarioCreador { get; set; }
        public bool? PlanDeTratamientoVigencia { get; set; }
        public int EntidadId = 14;
    }
}
