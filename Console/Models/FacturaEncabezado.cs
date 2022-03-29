using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class FacturaEncabezado
    {
        public FacturaEncabezado()
        {
            CuentaFacturas = new HashSet<CuentaFactura>();
            FacturaDetalles = new HashSet<FacturaDetalle>();
            ReciboIngresos = new HashSet<ReciboIngreso>();
        }

        public int FacturaEncabezadoId { get; set; }
        public string? FacturaEncabezadoNcf { get; set; }
        public string? FacturaEncabezadoIdCliente { get; set; }
        public string? FacturaEncabezadoIdCajero { get; set; }
        public decimal? FacturaEncabezadoTotalBruto { get; set; }
        public decimal? FacturaEncabezadoTotalCobertura { get; set; }
        public decimal? FacturaEncabezadoTotalDiferencia { get; set; }
        public DateTime? FacturaEncabezadoFechaCreacion { get; set; }
        public int? FacturaEncabezadoIdUsuarioCreador { get; set; }
        public bool? FacturaEncabezadoVigencia { get; set; }

        public virtual Persona? FacturaEncabezadoIdCajeroNavigation { get; set; }
        public virtual Persona? FacturaEncabezadoIdClienteNavigation { get; set; }
        public virtual ICollection<CuentaFactura> CuentaFacturas { get; set; }
        public virtual ICollection<FacturaDetalle> FacturaDetalles { get; set; }
        public virtual ICollection<ReciboIngreso> ReciboIngresos { get; set; }
    }
}
