using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core_Console.Models
{
    public partial class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AperturaYcierreDeCaja> AperturaYcierreDeCajas { get; set; } = null!;
        public virtual DbSet<Aseguradora> Aseguradoras { get; set; } = null!;
        public virtual DbSet<Autorizacion> Autorizacions { get; set; } = null!;
        public virtual DbSet<Caja> Cajas { get; set; } = null!;
        public virtual DbSet<CajaUsuario> CajaUsuarios { get; set; } = null!;
        public virtual DbSet<Cuenta> Cuentas { get; set; } = null!;
        public virtual DbSet<CuentaFactura> CuentaFacturas { get; set; } = null!;
        public virtual DbSet<Egreso> Egresos { get; set; } = null!;
        public virtual DbSet<FacturaDetalle> FacturaDetalles { get; set; } = null!;
        public virtual DbSet<FacturaEncabezado> FacturaEncabezados { get; set; } = null!;
        public virtual DbSet<LogGeneral> LogGenerals { get; set; } = null!;
        public virtual DbSet<Pago> Pagos { get; set; } = null!;
        public virtual DbSet<Perfil> Perfils { get; set; } = null!;
        public virtual DbSet<Persona> Personas { get; set; } = null!;
        public virtual DbSet<PlanDeTratamiento> PlanDeTratamientos { get; set; } = null!;
        public virtual DbSet<ProcesoMedico> ProcesoMedicos { get; set; } = null!;
        public virtual DbSet<ReciboIngreso> ReciboIngresos { get; set; } = null!;
        public virtual DbSet<Tblerrore> Tblerrores { get; set; } = null!;
        public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; } = null!;
        public virtual DbSet<TipoPago> TipoPagos { get; set; } = null!;
        public virtual DbSet<TipoPersona> TipoPersonas { get; set; } = null!;
        public virtual DbSet<TipoProceso> TipoProcesos { get; set; } = null!;
        public virtual DbSet<Transaccione> Transacciones { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=TINEOPC\\MSSQLSERVER01;Initial Catalog=Hospital;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AperturaYcierreDeCaja>(entity =>
            {
                entity.ToTable("AperturaYCierreDeCaja");

                entity.Property(e => e.AperturaYcierreDeCajaId).HasColumnName("AperturaYCierreDeCaja_Id");

                entity.Property(e => e.AperturaYcierreDeCaja10pesos).HasColumnName("AperturaYCierreDeCaja_10Pesos");

                entity.Property(e => e.AperturaYcierreDeCaja1peso).HasColumnName("AperturaYCierreDeCaja_1Peso");

                entity.Property(e => e.AperturaYcierreDeCaja25pesos).HasColumnName("AperturaYCierreDeCaja_25Pesos");

                entity.Property(e => e.AperturaYcierreDeCaja5peso).HasColumnName("AperturaYCierreDeCaja_5Peso");

                entity.Property(e => e.AperturaYcierreDeCajaAperturaOcierre).HasColumnName("AperturaYCierreDeCaja_AperturaOCierre");

                entity.Property(e => e.AperturaYcierreDeCajaCienPesos).HasColumnName("AperturaYCierreDeCaja_CienPesos");

                entity.Property(e => e.AperturaYcierreDeCajaCincuentaPesos).HasColumnName("AperturaYCierreDeCaja_CincuentaPesos");

                entity.Property(e => e.AperturaYcierreDeCajaDeposito)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AperturaYCierreDeCaja_Deposito");

                entity.Property(e => e.AperturaYcierreDeCajaDosMilPesos).HasColumnName("AperturaYCierreDeCaja_DosMIlPesos");

                entity.Property(e => e.AperturaYcierreDeCajaDoscientosPesos).HasColumnName("AperturaYCierreDeCaja_DoscientosPesos");

                entity.Property(e => e.AperturaYcierreDeCajaFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("AperturaYCierreDeCaja_Fecha");

                entity.Property(e => e.AperturaYcierreDeCajaIdCaja).HasColumnName("AperturaYCierreDeCaja_IdCaja");

                entity.Property(e => e.AperturaYcierreDeCajaIdUsuarioCreador).HasColumnName("AperturaYCierreDeCaja_IdUsuarioCreador");

                entity.Property(e => e.AperturaYcierreDeCajaMilPesos).HasColumnName("AperturaYCierreDeCaja_MIlPesos");

                entity.Property(e => e.AperturaYcierreDeCajaQuinientosPeso).HasColumnName("AperturaYCierreDeCaja_QuinientosPeso");

                entity.Property(e => e.AperturaYcierreDeCajaTotalCheques)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AperturaYCierreDeCaja_TotalCheques");

                entity.Property(e => e.AperturaYcierreDeCajaTotalCredito)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AperturaYCierreDeCaja_TotalCredito");

                entity.Property(e => e.AperturaYcierreDeCajaTotalEfectivo).HasColumnName("AperturaYCierreDeCaja_TotalEfectivo");

                entity.Property(e => e.AperturaYcierreDeCajaTotalGeneral)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AperturaYCierreDeCaja_TotalGeneral");

                entity.Property(e => e.AperturaYcierreDeCajaTotalTarjeta)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AperturaYCierreDeCaja_TotalTarjeta");

                entity.Property(e => e.AperturaYcierreDeCajaTransferencia)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("AperturaYCierreDeCaja_Transferencia");

                entity.Property(e => e.AperturaYcierreDeCajaVigencia).HasColumnName("AperturaYCierreDeCaja_Vigencia");

                entity.HasOne(d => d.AperturaYcierreDeCajaIdCajaNavigation)
                    .WithMany(p => p.AperturaYcierreDeCajas)
                    .HasForeignKey(d => d.AperturaYcierreDeCajaIdCaja)
                    .HasConstraintName("FK_AperturaYCierreDeCaja.AperturaYCierreDeCaja_IdCaja");

                entity.HasOne(d => d.AperturaYcierreDeCajaIdUsuarioCreadorNavigation)
                    .WithMany(p => p.AperturaYcierreDeCajas)
                    .HasForeignKey(d => d.AperturaYcierreDeCajaIdUsuarioCreador)
                    .HasConstraintName("FK_AperturaYCierreDeCaja.AperturaYCierreDeCaja_IdUsuarioCreador");
            });

            modelBuilder.Entity<Aseguradora>(entity =>
            {
                entity.HasKey(e => e.AseguraodraId)
                    .HasName("PK__Asegurad__00DFD79A86D300CF");

                entity.ToTable("Aseguradora");

                entity.Property(e => e.AseguraodraId).HasColumnName("Aseguraodra_Id");

                entity.Property(e => e.AseguradoraDescripcion)
                    .HasMaxLength(250)
                    .HasColumnName("Aseguradora_Descripcion");

                entity.Property(e => e.AseguradoraFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Aseguradora_FechaCreacion");

                entity.Property(e => e.AseguradoraIdUsuarioCreador).HasColumnName("Aseguradora_IdUsuarioCreador");

                entity.Property(e => e.AseguradoraVigencia).HasColumnName("Aseguradora_Vigencia");
            });

            modelBuilder.Entity<Autorizacion>(entity =>
            {
                entity.ToTable("Autorizacion");

                entity.Property(e => e.AutorizacionId).HasColumnName("Autorizacion_Id");

                entity.Property(e => e.AutorizacionCobertura)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("Autorizacion_Cobertura");

                entity.Property(e => e.AutorizacionDiferencia)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Autorizacion_Diferencia");

                entity.Property(e => e.AutorizacionFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Autorizacion_FechaCreacion");

                entity.Property(e => e.AutorizacionIdAseguradora).HasColumnName("Autorizacion_IdAseguradora");

                entity.Property(e => e.AutorizacionIdProcedimiento).HasColumnName("Autorizacion_IdProcedimiento");

                entity.Property(e => e.AutorizacionIdUsuarioCreador).HasColumnName("Autorizacion_IdUsuarioCreador");

                entity.Property(e => e.AutorizacionNoAutorizacion).HasColumnName("Autorizacion_NoAutorizacion");

                entity.Property(e => e.AutorizacionPrecio)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Autorizacion_Precio");

                entity.Property(e => e.AutorizacionVigencia).HasColumnName("Autorizacion_Vigencia");

                entity.HasOne(d => d.AutorizacionIdAseguradoraNavigation)
                    .WithMany(p => p.Autorizacions)
                    .HasForeignKey(d => d.AutorizacionIdAseguradora)
                    .HasConstraintName("FK_Autorizacion.Autorizacion_IdAseguradora");

                entity.HasOne(d => d.AutorizacionIdProcedimientoNavigation)
                    .WithMany(p => p.Autorizacions)
                    .HasForeignKey(d => d.AutorizacionIdProcedimiento)
                    .HasConstraintName("FK_Autorizacion.Autorizacion_IdProcedimiento");
            });

            modelBuilder.Entity<Caja>(entity =>
            {
                entity.ToTable("Caja");

                entity.Property(e => e.CajaId).HasColumnName("Caja_Id");

                entity.Property(e => e.CajaDescripcion)
                    .HasMaxLength(250)
                    .HasColumnName("Caja_Descripcion");

                entity.Property(e => e.CajaFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Caja_FechaCreacion");

                entity.Property(e => e.CajaIdUsuarioCreador).HasColumnName("Caja_IdUsuarioCreador");

                entity.Property(e => e.CajaVigencia).HasColumnName("Caja_Vigencia");
            });

            modelBuilder.Entity<CajaUsuario>(entity =>
            {
                entity.HasKey(e => new { e.CajaUsuarioIdUsuario, e.CajaUsuarioIdCaja })
                    .HasName("PK__Caja_Usu__C25DB73AC61A2743");

                entity.ToTable("Caja_Usuario");

                entity.Property(e => e.CajaUsuarioIdUsuario).HasColumnName("Caja_Usuario_IdUsuario");

                entity.Property(e => e.CajaUsuarioIdCaja).HasColumnName("Caja_Usuario_IdCaja");

                entity.Property(e => e.CajaUsuarioIdUsuarioFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Caja_Usuario_IdUsuario_FechaCreacion");

                entity.Property(e => e.CajaUsuarioIdUsuarioIdUsuarioCreador).HasColumnName("Caja_Usuario_IdUsuario_IdUsuarioCreador");

                entity.Property(e => e.CajaUsuarioIdUsuarioVigencia).HasColumnName("Caja_Usuario_IdUsuario_Vigencia");

                entity.HasOne(d => d.CajaUsuarioIdCajaNavigation)
                    .WithMany(p => p.CajaUsuarios)
                    .HasForeignKey(d => d.CajaUsuarioIdCaja)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Caja_Usuario.Caja_Usuario_IdCaja");

                entity.HasOne(d => d.CajaUsuarioIdUsuarioNavigation)
                    .WithMany(p => p.CajaUsuarios)
                    .HasForeignKey(d => d.CajaUsuarioIdUsuario)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Caja_Usuario.Caja_Usuario_IdUsuario");
            });

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.Property(e => e.CuentaId).HasColumnName("Cuenta_Id");

                entity.Property(e => e.CuentaBalance)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Cuenta_Balance");

                entity.Property(e => e.CuentaFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Cuenta_FechaCreacion");

                entity.Property(e => e.CuentaIdPersona)
                    .HasMaxLength(20)
                    .HasColumnName("Cuenta_IdPersona");

                entity.Property(e => e.CuentaIdUsuarioCreador).HasColumnName("Cuenta_IdUsuarioCreador");

                entity.Property(e => e.CuentaVigencia).HasColumnName("Cuenta_Vigencia");

                entity.HasOne(d => d.CuentaIdPersonaNavigation)
                    .WithMany(p => p.Cuenta)
                    .HasForeignKey(d => d.CuentaIdPersona)
                    .HasConstraintName("FK_Cuentas.Cuenta_IdPersona");
            });

            modelBuilder.Entity<CuentaFactura>(entity =>
            {
                entity.HasKey(e => new { e.CuentaFacturaIdFactura, e.CuentaFacturaIdCuenta })
                    .HasName("PK__Cuenta_F__64B6D522C72DEF9F");

                entity.ToTable("Cuenta_Factura");

                entity.Property(e => e.CuentaFacturaIdFactura).HasColumnName("Cuenta_Factura_IdFactura");

                entity.Property(e => e.CuentaFacturaIdCuenta).HasColumnName("Cuenta_Factura_IdCuenta");

                entity.Property(e => e.CuentaFacturaDUsuarioCreador).HasColumnName("Cuenta_Factura_dUsuarioCreador");

                entity.Property(e => e.CuentaFacturaFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Cuenta_Factura_FechaCreacion");

                entity.Property(e => e.CuentaFacturaMonto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Cuenta_Factura_Monto");

                entity.Property(e => e.CuentaFacturaVigencia).HasColumnName("Cuenta_Factura_Vigencia");

                entity.HasOne(d => d.CuentaFacturaIdCuentaNavigation)
                    .WithMany(p => p.CuentaFacturas)
                    .HasForeignKey(d => d.CuentaFacturaIdCuenta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cuenta_Factura.Cuenta_Factura_IdCuenta");

                entity.HasOne(d => d.CuentaFacturaIdFacturaNavigation)
                    .WithMany(p => p.CuentaFacturas)
                    .HasForeignKey(d => d.CuentaFacturaIdFactura)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cuenta_Factura.Cuenta_Factura_IdFactura");
            });

            modelBuilder.Entity<Egreso>(entity =>
            {
                entity.ToTable("Egreso");

                entity.Property(e => e.EgresoId).HasColumnName("Egreso_Id");

                entity.Property(e => e.EgresoCedula)
                    .HasMaxLength(50)
                    .HasColumnName("Egreso_Cedula");

                entity.Property(e => e.EgresoConcepto)
                    .HasMaxLength(50)
                    .HasColumnName("Egreso_Concepto");

                entity.Property(e => e.EgresoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Egreso_FechaCreacion");

                entity.Property(e => e.EgresoIdUsuarioCreador).HasColumnName("Egreso_IdUsuarioCreador");

                entity.Property(e => e.EgresoMonto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Egreso_Monto");

                entity.Property(e => e.EgresoPagadoA)
                    .HasMaxLength(50)
                    .HasColumnName("Egreso_PagadoA");

                entity.Property(e => e.EgresoPreparado)
                    .HasMaxLength(50)
                    .HasColumnName("Egreso_Preparado");

                entity.Property(e => e.EgresoRecibido)
                    .HasMaxLength(50)
                    .HasColumnName("Egreso_Recibido");

                entity.Property(e => e.EgresoVigencia).HasColumnName("Egreso_Vigencia");

                entity.Property(e => e.EgrespAprobado)
                    .HasMaxLength(50)
                    .HasColumnName("Egresp_Aprobado");

                entity.HasOne(d => d.EgresoIdUsuarioCreadorNavigation)
                    .WithMany(p => p.Egresos)
                    .HasForeignKey(d => d.EgresoIdUsuarioCreador)
                    .HasConstraintName("FK_Egreso.Egreso_IdUsuarioCreador");
            });

            modelBuilder.Entity<FacturaDetalle>(entity =>
            {
                entity.ToTable("FacturaDetalle");

                entity.Property(e => e.FacturaDetalleId).HasColumnName("FacturaDetalle_Id");

                entity.Property(e => e.FacturaDetalleFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FacturaDetalle_FechaCreacion");

                entity.Property(e => e.FacturaDetalleIdFactura).HasColumnName("FacturaDetalle_IdFactura");

                entity.Property(e => e.FacturaDetalleIdUsuarioCreador).HasColumnName("FacturaDetalle_IdUsuarioCreador");

                entity.Property(e => e.FacturaDetalleMontoBruto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FacturaDetalle_MontoBruto");

                entity.Property(e => e.FacturaDetalleMontoCobertura)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FacturaDetalle_MontoCobertura");

                entity.Property(e => e.FacturaDetalleMontoDescuento)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FacturaDetalle_MontoDescuento");

                entity.Property(e => e.FacturaDetalleMontoItbis)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("FacturaDetalle_MontoItbis");

                entity.Property(e => e.FacturaDetalleMontoTotal)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FacturaDetalle_MontoTotal");

                entity.Property(e => e.FacturaDetallePlanDeTratamiento).HasColumnName("FacturaDetalle_PlanDeTratamiento");

                entity.Property(e => e.FacturaDetalleVigencia).HasColumnName("FacturaDetalle_Vigencia");

                entity.HasOne(d => d.FacturaDetalleIdFacturaNavigation)
                    .WithMany(p => p.FacturaDetalles)
                    .HasForeignKey(d => d.FacturaDetalleIdFactura)
                    .HasConstraintName("FK_FacturaDetalle.FacturaDetalle_IdFactura");

                entity.HasOne(d => d.FacturaDetallePlanDeTratamientoNavigation)
                    .WithMany(p => p.FacturaDetalles)
                    .HasForeignKey(d => d.FacturaDetallePlanDeTratamiento)
                    .HasConstraintName("FK_FacturaDetalle.FacturaDetalle_PlanDeTratamiento");
            });

            modelBuilder.Entity<FacturaEncabezado>(entity =>
            {
                entity.ToTable("FacturaEncabezado");

                entity.Property(e => e.FacturaEncabezadoId).HasColumnName("FacturaEncabezado_Id");

                entity.Property(e => e.FacturaEncabezadoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("FacturaEncabezado_FechaCreacion");

                entity.Property(e => e.FacturaEncabezadoIdCajero)
                    .HasMaxLength(20)
                    .HasColumnName("FacturaEncabezado_IdCajero");

                entity.Property(e => e.FacturaEncabezadoIdCliente)
                    .HasMaxLength(20)
                    .HasColumnName("FacturaEncabezado_IdCliente");

                entity.Property(e => e.FacturaEncabezadoIdUsuarioCreador).HasColumnName("FacturaEncabezado_IdUsuarioCreador");

                entity.Property(e => e.FacturaEncabezadoNcf)
                    .HasMaxLength(50)
                    .HasColumnName("FacturaEncabezado_NCF");

                entity.Property(e => e.FacturaEncabezadoTotalBruto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FacturaEncabezado_TotalBruto");

                entity.Property(e => e.FacturaEncabezadoTotalCobertura)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FacturaEncabezado_TotalCobertura");

                entity.Property(e => e.FacturaEncabezadoTotalDiferencia)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("FacturaEncabezado_TotalDiferencia");

                entity.Property(e => e.FacturaEncabezadoVigencia).HasColumnName("FacturaEncabezado_Vigencia");

                entity.HasOne(d => d.FacturaEncabezadoIdCajeroNavigation)
                    .WithMany(p => p.FacturaEncabezadoFacturaEncabezadoIdCajeroNavigations)
                    .HasForeignKey(d => d.FacturaEncabezadoIdCajero)
                    .HasConstraintName("FK_FacturaEncabezado.FacturaEncabezado_IdCajero");

                entity.HasOne(d => d.FacturaEncabezadoIdClienteNavigation)
                    .WithMany(p => p.FacturaEncabezadoFacturaEncabezadoIdClienteNavigations)
                    .HasForeignKey(d => d.FacturaEncabezadoIdCliente)
                    .HasConstraintName("FK_FacturaEncabezado.FacturaEncabezado_IdCliente");
            });

            modelBuilder.Entity<LogGeneral>(entity =>
            {
                entity.ToTable("LogGeneral");

                entity.Property(e => e.LogGeneralId).HasColumnName("LogGeneral_Id");

                entity.Property(e => e.LogGeneralAccion)
                    .HasMaxLength(250)
                    .HasColumnName("LogGeneral_Accion");

                entity.Property(e => e.LogGeneralFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("LogGeneral_Fecha");

                entity.Property(e => e.LogGeneralIdUsuario).HasColumnName("LogGeneral_IdUsuario");

                entity.Property(e => e.LogGeneralMensaje)
                    .HasMaxLength(250)
                    .HasColumnName("LogGeneral_Mensaje");

                entity.Property(e => e.LogGeneralPantalla)
                    .HasMaxLength(250)
                    .HasColumnName("LogGeneral_Pantalla");

                entity.Property(e => e.LogGeneralVigencia).HasColumnName("LogGeneral_Vigencia");

                entity.HasOne(d => d.LogGeneralIdUsuarioNavigation)
                    .WithMany(p => p.LogGenerals)
                    .HasForeignKey(d => d.LogGeneralIdUsuario)
                    .HasConstraintName("FK_LogGeneral.LogGeneral_IdUsuario");
            });

            modelBuilder.Entity<Pago>(entity =>
            {
                entity.ToTable("Pago");

                entity.Property(e => e.PagoId).HasColumnName("Pago_Id");

                entity.Property(e => e.PagoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Pago_FechaCreacion");

                entity.Property(e => e.PagoIdCuenta).HasColumnName("Pago_IdCuenta");

                entity.Property(e => e.PagoIdPersona)
                    .HasMaxLength(20)
                    .HasColumnName("Pago_IdPersona");

                entity.Property(e => e.PagoIdUsuarioCreador).HasColumnName("Pago_IdUsuarioCreador");

                entity.Property(e => e.PagoMonto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("Pago_Monto");

                entity.Property(e => e.PagoReferencia)
                    .HasMaxLength(50)
                    .HasColumnName("Pago_Referencia");

                entity.Property(e => e.PagoTipoPago).HasColumnName("Pago_TipoPago");

                entity.Property(e => e.PagoVigencia).HasColumnName("Pago_Vigencia");

                entity.HasOne(d => d.PagoIdCuentaNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.PagoIdCuenta)
                    .HasConstraintName("FK_Pago.Pago_IdCuenta");

                entity.HasOne(d => d.PagoIdPersonaNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.PagoIdPersona)
                    .HasConstraintName("FK_Pago.Pago_IdPersona");

                entity.HasOne(d => d.PagoIdUsuarioCreadorNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.PagoIdUsuarioCreador)
                    .HasConstraintName("FK_Pago.Pago_IdUsuarioCreador");

                entity.HasOne(d => d.PagoTipoPagoNavigation)
                    .WithMany(p => p.Pagos)
                    .HasForeignKey(d => d.PagoTipoPago)
                    .HasConstraintName("FK_Pago.Pago_TipoPago");
            });

            modelBuilder.Entity<Perfil>(entity =>
            {
                entity.ToTable("Perfil");

                entity.Property(e => e.PerfilId).HasColumnName("Perfil_Id");

                entity.Property(e => e.PerfilDescripcion)
                    .HasMaxLength(250)
                    .HasColumnName("Perfil_Descripcion");

                entity.Property(e => e.PerfilFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Perfil_FechaCreacion");

                entity.Property(e => e.PerfilIdUsuarioCreador).HasColumnName("Perfil_IdUsuarioCreador");

                entity.Property(e => e.PerfilVigencia).HasColumnName("Perfil_Vigencia");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.PersonaDocumento)
                    .HasName("PK__Persona__8AD76EC84C0E43F2");

                entity.ToTable("Persona");

                entity.Property(e => e.PersonaDocumento)
                    .HasMaxLength(20)
                    .HasColumnName("Persona_Documento");

                entity.Property(e => e.PersonaApellido)
                    .HasMaxLength(250)
                    .HasColumnName("Persona_Apellido");

                entity.Property(e => e.PersonaCorreoElectronico)
                    .HasMaxLength(50)
                    .HasColumnName("Persona_CorreoElectronico");

                entity.Property(e => e.PersonaDireccion)
                    .HasMaxLength(250)
                    .HasColumnName("Persona_Direccion");

                entity.Property(e => e.PersonaFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Persona_FechaCreacion");

                entity.Property(e => e.PersonaIdAseguradora).HasColumnName("Persona_IdAseguradora");

                entity.Property(e => e.PersonaIdUsuarioCreador).HasColumnName("Persona_IdUsuarioCreador");

                entity.Property(e => e.PersonaNombre)
                    .HasMaxLength(250)
                    .HasColumnName("Persona_Nombre");

                entity.Property(e => e.PersonaTelefono)
                    .HasMaxLength(50)
                    .HasColumnName("Persona_Telefono");

                entity.Property(e => e.PersonaTipoDocumento).HasColumnName("Persona_TipoDocumento");

                entity.Property(e => e.PersonaTipoPersona).HasColumnName("Persona_TipoPersona");

                entity.Property(e => e.PersonaVigencia).HasColumnName("Persona_Vigencia");

                entity.HasOne(d => d.PersonaIdAseguradoraNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.PersonaIdAseguradora)
                    .HasConstraintName("FK_Persona.Persona_IdAseguradora");

                entity.HasOne(d => d.PersonaTipoDocumentoNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.PersonaTipoDocumento)
                    .HasConstraintName("FK_Persona.Persona_TipoDocumento");

                entity.HasOne(d => d.PersonaTipoPersonaNavigation)
                    .WithMany(p => p.Personas)
                    .HasForeignKey(d => d.PersonaTipoPersona)
                    .HasConstraintName("FK_Persona.Persona_TipoPersona");
            });

            modelBuilder.Entity<PlanDeTratamiento>(entity =>
            {
                entity.ToTable("PlanDeTratamiento");

                entity.Property(e => e.PlanDeTratamientoId).HasColumnName("PlanDeTratamiento_Id");

                entity.Property(e => e.PlanDeTratamientoCausa)
                    .HasColumnType("text")
                    .HasColumnName("PlanDeTratamiento_Causa");

                entity.Property(e => e.PlanDeTratamientoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("PlanDeTratamiento_FechaCreacion");

                entity.Property(e => e.PlanDeTratamientoIdMedico)
                    .HasMaxLength(20)
                    .HasColumnName("PlanDeTratamiento_IdMedico");

                entity.Property(e => e.PlanDeTratamientoIdPaciente)
                    .HasMaxLength(20)
                    .HasColumnName("PlanDeTratamiento_IdPaciente");

                entity.Property(e => e.PlanDeTratamientoIdProcedimiento).HasColumnName("PlanDeTratamiento_IdProcedimiento");

                entity.Property(e => e.PlanDeTratamientoIdUsuarioCreador).HasColumnName("PlanDeTratamiento_IdUsuarioCreador");

                entity.Property(e => e.PlanDeTratamientoImagen).HasColumnName("PlanDeTratamiento_Imagen");

                entity.Property(e => e.PlanDeTratamientoNoAutorizacion).HasColumnName("PlanDeTratamiento_NoAutorizacion");

                entity.Property(e => e.PlanDeTratamientoResultado)
                    .HasMaxLength(50)
                    .HasColumnName("PlanDeTratamiento_Resultado");

                entity.Property(e => e.PlanDeTratamientoVigencia).HasColumnName("PlanDeTratamiento_Vigencia");

                entity.HasOne(d => d.PlanDeTratamientoIdMedicoNavigation)
                    .WithMany(p => p.PlanDeTratamientoPlanDeTratamientoIdMedicoNavigations)
                    .HasForeignKey(d => d.PlanDeTratamientoIdMedico)
                    .HasConstraintName("FK_PlanDeTratamiento.PlanDeTratamiento_IdMedico");

                entity.HasOne(d => d.PlanDeTratamientoIdPacienteNavigation)
                    .WithMany(p => p.PlanDeTratamientoPlanDeTratamientoIdPacienteNavigations)
                    .HasForeignKey(d => d.PlanDeTratamientoIdPaciente)
                    .HasConstraintName("FK_PlanDeTratamiento.PlanDeTratamiento_IdPaciente");

                entity.HasOne(d => d.PlanDeTratamientoIdProcedimientoNavigation)
                    .WithMany(p => p.PlanDeTratamientos)
                    .HasForeignKey(d => d.PlanDeTratamientoIdProcedimiento)
                    .HasConstraintName("FK_PlanDeTratamiento.PlanDeTratamiento_IdProcedimiento");
            });

            modelBuilder.Entity<ProcesoMedico>(entity =>
            {
                entity.ToTable("ProcesoMedico");

                entity.Property(e => e.ProcesoMedicoId).HasColumnName("ProcesoMedico_Id");

                entity.Property(e => e.ProcesoMedicoDescripcion).HasColumnName("ProcesoMedico_Descripcion");

                entity.Property(e => e.ProcesoMedicoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("ProcesoMedico_FechaCreacion");

                entity.Property(e => e.ProcesoMedicoIdTipoProceso).HasColumnName("ProcesoMedico_IdTipoProceso");

                entity.Property(e => e.ProcesoMedicoIdUsuarioCreador).HasColumnName("ProcesoMedico_IdUsuarioCreador");

                entity.Property(e => e.ProcesoMedicoPrecio)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ProcesoMedico_Precio");

                entity.Property(e => e.ProcesoMedicoVigencia).HasColumnName("ProcesoMedico_Vigencia");

                entity.HasOne(d => d.ProcesoMedicoIdTipoProcesoNavigation)
                    .WithMany(p => p.ProcesoMedicos)
                    .HasForeignKey(d => d.ProcesoMedicoIdTipoProceso)
                    .HasConstraintName("FK_ProcesoMedico.ProcesoMedico_IdTipoProceso");
            });

            modelBuilder.Entity<ReciboIngreso>(entity =>
            {
                entity.ToTable("ReciboIngreso");

                entity.Property(e => e.ReciboIngresoId).HasColumnName("ReciboIngreso_Id");

                entity.Property(e => e.ReciboIngresoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("ReciboIngreso_FechaCreacion");

                entity.Property(e => e.ReciboIngresoIdCajero)
                    .HasMaxLength(20)
                    .HasColumnName("ReciboIngreso_IdCajero");

                entity.Property(e => e.ReciboIngresoIdCuentaDetalle).HasColumnName("ReciboIngreso_IdCuentaDetalle");

                entity.Property(e => e.ReciboIngresoIdPaciente)
                    .HasMaxLength(20)
                    .HasColumnName("ReciboIngreso_IdPaciente");

                entity.Property(e => e.ReciboIngresoIdUsuarioCreador).HasColumnName("ReciboIngreso_IdUsuarioCreador");

                entity.Property(e => e.ReciboIngresoMonto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("ReciboIngreso_Monto");

                entity.Property(e => e.ReciboIngresoVigencia).HasColumnName("ReciboIngreso_Vigencia");

                entity.HasOne(d => d.ReciboIngresoIdCajeroNavigation)
                    .WithMany(p => p.ReciboIngresoReciboIngresoIdCajeroNavigations)
                    .HasForeignKey(d => d.ReciboIngresoIdCajero)
                    .HasConstraintName("FK_ReciboIngreso.ReciboIngreso_IdCajero");

                entity.HasOne(d => d.ReciboIngresoIdCuentaDetalleNavigation)
                    .WithMany(p => p.ReciboIngresos)
                    .HasForeignKey(d => d.ReciboIngresoIdCuentaDetalle)
                    .HasConstraintName("FK_ReciboIngreso.ReciboIngreso_IsCuentaDetalle");

                entity.HasOne(d => d.ReciboIngresoIdPacienteNavigation)
                    .WithMany(p => p.ReciboIngresoReciboIngresoIdPacienteNavigations)
                    .HasForeignKey(d => d.ReciboIngresoIdPaciente)
                    .HasConstraintName("FK_ReciboIngreso.ReciboIngreso_IdPaciente");
            });

            modelBuilder.Entity<Tblerrore>(entity =>
            {
                entity.HasKey(e => e.TblerroresId)
                    .HasName("PK__TBLError__1032454A30CFD0CE");

                entity.ToTable("TBLErrores");

                entity.Property(e => e.TblerroresId).HasColumnName("TBLErrores_Id");

                entity.Property(e => e.TblerroresIdUsuario).HasColumnName("TBLErrores_IdUsuario");

                entity.Property(e => e.TblerroresMensaje)
                    .HasMaxLength(250)
                    .HasColumnName("TBLErrores_Mensaje");

                entity.Property(e => e.TblerroresMetodo)
                    .HasMaxLength(250)
                    .HasColumnName("TBLErrores_Metodo");

                entity.Property(e => e.TblerroresPantalla)
                    .HasMaxLength(250)
                    .HasColumnName("TBLErrores_Pantalla");

                entity.Property(e => e.TblerroresVigencia).HasColumnName("TBLErrores_Vigencia");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TipoDocumento");

                entity.Property(e => e.TipoDocumentoId).HasColumnName("TipoDocumento_Id");

                entity.Property(e => e.TipoDocumentoDescripcion)
                    .HasMaxLength(250)
                    .HasColumnName("TipoDocumento_Descripcion");

                entity.Property(e => e.TipoDocumentoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("TipoDocumento_FechaCreacion");

                entity.Property(e => e.TipoDocumentoIdUsuarioCreador).HasColumnName("TipoDocumento_IdUsuarioCreador");

                entity.Property(e => e.TipoDocumentoVigencia).HasColumnName("TipoDocumento_Vigencia");
            });

            modelBuilder.Entity<TipoPago>(entity =>
            {
                entity.ToTable("TipoPago");

                entity.Property(e => e.TipoPagoId).HasColumnName("TipoPago_Id");

                entity.Property(e => e.TipoPagoDescripcion)
                    .HasMaxLength(250)
                    .HasColumnName("TipoPago_Descripcion");

                entity.Property(e => e.TipoPagoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("TipoPago_FechaCreacion");

                entity.Property(e => e.TipoPagoIdUsuarioCreador).HasColumnName("TipoPago_IdUsuarioCreador");

                entity.Property(e => e.TipoPagoVigencia).HasColumnName("TipoPago_Vigencia");
            });

            modelBuilder.Entity<TipoPersona>(entity =>
            {
                entity.ToTable("TipoPersona");

                entity.Property(e => e.TipoPersonaId).HasColumnName("TipoPersona_Id");

                entity.Property(e => e.TipoPersonaDescripcion)
                    .HasMaxLength(250)
                    .HasColumnName("TipoPersona_Descripcion");

                entity.Property(e => e.TipoPersonaFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("TipoPersona_FechaCreacion");

                entity.Property(e => e.TipoPersonaIdUsuarioCreador).HasColumnName("TipoPersona_IdUsuarioCreador");

                entity.Property(e => e.TipoPersonaVigencia).HasColumnName("TipoPersona_Vigencia");
            });

            modelBuilder.Entity<TipoProceso>(entity =>
            {
                entity.ToTable("TipoProceso");

                entity.Property(e => e.TipoProcesoId).HasColumnName("TipoProceso_Id");

                entity.Property(e => e.TipoProcesoDescripcion)
                    .HasMaxLength(250)
                    .HasColumnName("TipoProceso_Descripcion");

                entity.Property(e => e.TipoProcesoFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("TipoProceso_FechaCreacion");

                entity.Property(e => e.TipoProcesoIdUsuarioCreador).HasColumnName("TipoProceso_IdUsuarioCreador");

                entity.Property(e => e.TipoProcesoVigencia).HasColumnName("TipoProceso_Vigencia");
            });

            modelBuilder.Entity<Transaccione>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Comentario)
                    .HasColumnType("text")
                    .HasColumnName("comentario");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("createdAt")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DebitoCredito).HasColumnName("debito_credito");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Monto)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("monto");

                entity.Property(e => e.NumeroCuenta)
                    .HasMaxLength(50)
                    .HasColumnName("numero_cuenta");

                entity.Property(e => e.TipoTransaccion).HasColumnName("tipo_transaccion");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.UsuarioId).HasColumnName("Usuario_Id");

                entity.Property(e => e.IdPersona).HasMaxLength(20);

                entity.Property(e => e.UsuarioContraseña)
                    .HasMaxLength(250)
                    .HasColumnName("Usuario_Contraseña");

                entity.Property(e => e.UsuarioFechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("Usuario_FechaCreacion");

                entity.Property(e => e.UsuarioIdPerfil).HasColumnName("Usuario_IdPerfil");

                entity.Property(e => e.UsuarioIdUsuarioCreador).HasColumnName("Usuario_IdUsuarioCreador");

                entity.Property(e => e.UsuarioNickname)
                    .HasMaxLength(250)
                    .HasColumnName("Usuario_Nickname");

                entity.Property(e => e.UsuarioVigencia).HasColumnName("Usuario_Vigencia");

                entity.HasOne(d => d.IdPersonaNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.IdPersona)
                    .HasConstraintName("FK_Usuarios.Usuario_IdPersona");

                entity.HasOne(d => d.UsuarioIdPerfilNavigation)
                    .WithMany(p => p.Usuarios)
                    .HasForeignKey(d => d.UsuarioIdPerfil)
                    .HasConstraintName("FK_Usuarios.Usuario_IdPerfil");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
