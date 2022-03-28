using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Core_Console.Models
{
    public partial class CoreContext : DbContext
    {
        public CoreContext()
        {
        }

        public CoreContext(DbContextOptions<CoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Aseguradora> Aseguradoras { get; set; } = null!;
        public virtual DbSet<Autorizacione> Autorizaciones { get; set; } = null!;
        public virtual DbSet<Cuenta> Cuentas { get; set; } = null!;
        public virtual DbSet<Paciente> Pacientes { get; set; } = null!;
        public virtual DbSet<Perfile> Perfiles { get; set; } = null!;
        public virtual DbSet<Procedimiento> Procedimientos { get; set; } = null!;
        public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; } = null!;
        public virtual DbSet<TipoPersona> TipoPersonas { get; set; } = null!;
        public virtual DbSet<TipoProceso> TipoProcesos { get; set; } = null!;
        public virtual DbSet<Usuario> Usuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\chris\\source\\repos\\Core_Hospital\\Console\\Database.mdf;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Aseguradora>(entity =>
            {
                entity.ToTable("Aseguradora");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre).HasMaxLength(250);

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Autorizacione>(entity =>
            {
                entity.Property(e => e.AseguradoraId).HasColumnName("Aseguradora_Id");

                entity.Property(e => e.Cobertura).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Diferencia).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProcedimientoId).HasColumnName("Procedimiento_Id");

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Cuenta>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.PersonaId })
                    .HasName("PK__Cuentas__20DDEB5B578C31E6");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.PersonaId).HasColumnName("Persona_Id");

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Paciente>(entity =>
            {
                entity.HasKey(e => new { e.TipoDocumento, e.Documento });

                entity.Property(e => e.Documento).HasMaxLength(20);

                entity.Property(e => e.Apellidos).HasMaxLength(250);

                entity.Property(e => e.AseguradoraId).HasColumnName("Aseguradora_Id");

                entity.Property(e => e.CorreoElectronico).HasMaxLength(250);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Direccion).HasMaxLength(250);

                entity.Property(e => e.Nombres).HasMaxLength(250);

                entity.Property(e => e.Telefono).HasMaxLength(12);

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Perfile>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descripcion).HasMaxLength(250);

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Procedimiento>(entity =>
            {
                entity.ToTable("Procedimiento");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descripcion).HasMaxLength(250);

                entity.Property(e => e.Precio).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TipoProcesoId).HasColumnName("TipoProceso_Id");

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.ToTable("TipoDocumento");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nombre).HasMaxLength(50);
            });

            modelBuilder.Entity<TipoPersona>(entity =>
            {
                entity.ToTable("TipoPersona");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descripcion).HasMaxLength(250);

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<TipoProceso>(entity =>
            {
                entity.ToTable("TipoProceso");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Descripcion).HasMaxLength(255);

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Contraseña).HasMaxLength(200);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Nickname).HasMaxLength(50);

                entity.Property(e => e.PerfilId).HasColumnName("Perfil_Id");

                entity.Property(e => e.UsuarioCreadorId).HasColumnName("UsuarioCreador_Id");

                entity.Property(e => e.Vigencia)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
