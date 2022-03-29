using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class Persona
    {
        public Persona()
        {
            Cuenta = new HashSet<Cuenta>();
            FacturaEncabezadoFacturaEncabezadoIdCajeroNavigations = new HashSet<FacturaEncabezado>();
            FacturaEncabezadoFacturaEncabezadoIdClienteNavigations = new HashSet<FacturaEncabezado>();
            Pagos = new HashSet<Pago>();
            PlanDeTratamientoPlanDeTratamientoIdMedicoNavigations = new HashSet<PlanDeTratamiento>();
            PlanDeTratamientoPlanDeTratamientoIdPacienteNavigations = new HashSet<PlanDeTratamiento>();
            ReciboIngresoReciboIngresoIdCajeroNavigations = new HashSet<ReciboIngreso>();
            ReciboIngresoReciboIngresoIdPacienteNavigations = new HashSet<ReciboIngreso>();
            Usuarios = new HashSet<Usuario>();
        }

        public int? PersonaTipoDocumento { get; set; }
        public string PersonaDocumento { get; set; } = null!;
        public string? PersonaNombre { get; set; }
        public string? PersonaApellido { get; set; }
        public int? PersonaTipoPersona { get; set; }
        public string? PersonaCorreoElectronico { get; set; }
        public string? PersonaTelefono { get; set; }
        public string? PersonaDireccion { get; set; }
        public int? PersonaIdAseguradora { get; set; }
        public DateTime? PersonaFechaCreacion { get; set; }
        public int? PersonaIdUsuarioCreador { get; set; }
        public bool? PersonaVigencia { get; set; }

        public virtual Aseguradora? PersonaIdAseguradoraNavigation { get; set; }
        public virtual TipoDocumento? PersonaTipoDocumentoNavigation { get; set; }
        public virtual TipoPersona? PersonaTipoPersonaNavigation { get; set; }
        public virtual ICollection<Cuenta> Cuenta { get; set; }
        public virtual ICollection<FacturaEncabezado> FacturaEncabezadoFacturaEncabezadoIdCajeroNavigations { get; set; }
        public virtual ICollection<FacturaEncabezado> FacturaEncabezadoFacturaEncabezadoIdClienteNavigations { get; set; }
        public virtual ICollection<Pago> Pagos { get; set; }
        public virtual ICollection<PlanDeTratamiento> PlanDeTratamientoPlanDeTratamientoIdMedicoNavigations { get; set; }
        public virtual ICollection<PlanDeTratamiento> PlanDeTratamientoPlanDeTratamientoIdPacienteNavigations { get; set; }
        public virtual ICollection<ReciboIngreso> ReciboIngresoReciboIngresoIdCajeroNavigations { get; set; }
        public virtual ICollection<ReciboIngreso> ReciboIngresoReciboIngresoIdPacienteNavigations { get; set; }
        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
