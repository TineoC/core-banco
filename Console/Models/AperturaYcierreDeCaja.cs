using System;
using System.Collections.Generic;

namespace Core_Console.Models
{
    public partial class AperturaYcierreDeCaja
    {
        public int AperturaYcierreDeCajaId { get; set; }
        public int? AperturaYcierreDeCajaIdCaja { get; set; }
        public int? AperturaYcierreDeCajaDosMilPesos { get; set; }
        public int? AperturaYcierreDeCajaMilPesos { get; set; }
        public int? AperturaYcierreDeCajaQuinientosPeso { get; set; }
        public int? AperturaYcierreDeCajaDoscientosPesos { get; set; }
        public int? AperturaYcierreDeCajaCienPesos { get; set; }
        public int? AperturaYcierreDeCajaCincuentaPesos { get; set; }
        public int? AperturaYcierreDeCaja25pesos { get; set; }
        public int? AperturaYcierreDeCaja10pesos { get; set; }
        public int? AperturaYcierreDeCaja5peso { get; set; }
        public int? AperturaYcierreDeCaja1peso { get; set; }
        public int? AperturaYcierreDeCajaTotalEfectivo { get; set; }
        public decimal? AperturaYcierreDeCajaTotalCredito { get; set; }
        public decimal? AperturaYcierreDeCajaTotalTarjeta { get; set; }
        public decimal? AperturaYcierreDeCajaTransferencia { get; set; }
        public decimal? AperturaYcierreDeCajaDeposito { get; set; }
        public decimal? AperturaYcierreDeCajaTotalCheques { get; set; }
        public decimal? AperturaYcierreDeCajaTotalGeneral { get; set; }
        public bool? AperturaYcierreDeCajaAperturaOcierre { get; set; }
        public DateTime? AperturaYcierreDeCajaFecha { get; set; }
        public int? AperturaYcierreDeCajaIdUsuarioCreador { get; set; }
        public bool? AperturaYcierreDeCajaVigencia { get; set; }

        public virtual Caja? AperturaYcierreDeCajaIdCajaNavigation { get; set; }
        public virtual Usuario? AperturaYcierreDeCajaIdUsuarioCreadorNavigation { get; set; }
    }
}
