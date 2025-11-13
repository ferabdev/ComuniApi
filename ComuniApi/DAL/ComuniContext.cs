using ComuniApi.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System;

namespace ComuniApi.DAL
{
    public class ComuniContext : DbContext
    {
        public ComuniContext(DbContextOptions<ComuniContext> options) : base(options) { }

        public DbSet<EdoCuentaEntity> EdoCuentas { get; set; }
        public DbSet<ConceptoEntity> Conceptos { get; set; }
        public DbSet<ComunidadEntity> Comunidades { get; set; }
        public DbSet<UsuarioEntity> Usuarios { get; set; }
        public DbSet<RolEntity> Roles { get; set; }
        public DbSet<IncidenciaEntity> Incidencias { get; set; }
        public DbSet<IncidenciaEstatusEntity> IncidenciasEstatus { get; set; }
        public DbSet<ReporteEntity> Reportes { get; set; }
        public DbSet<ReporteEstatusEntity> ReportesEstatus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ComunidadEntity>(entity =>
            {
                entity.HasData(
                    new ComunidadEntity
                    {
                        Id = 1,
                        Nombre = "Comunidad test",
                        Direccion = "Direccion test",
                        Correo = "",
                        CodigoComunidad = "WUEBOS"
                    });

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Direccion).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.MaxUsers).IsRequired();

                //entidad tiene codigo de comunidad unico e irrepetible
                entity.HasIndex(e => e.CodigoComunidad).IsUnique();
                entity.Property(e => e.CodigoComunidad).IsRequired().HasMaxLength(6).HasDefaultValue("-");

                entity.HasMany(e => e.Usuarios)
                      .WithOne(e => e.Comunidad)
                      .HasForeignKey(e => e.ComunidadId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<EdoCuentaEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Fecha).IsRequired();
                entity.Property(e => e.FechaLimite).IsRequired();
                entity.Property(e => e.Monto).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.UsuarioId).IsRequired();
                entity.Property(e => e.ConceptoId);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Usuario)
                      .WithMany(e => e.EdoCuentas)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Concepto)
                        .WithMany(e => e.EdoCuentas)
                        .HasForeignKey(e => e.ConceptoId)
                        .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UsuarioEntity>(entity =>
            {
                var usuarios = new List<UsuarioEntity>()
                {
                    new UsuarioEntity
                    {
                        Id = 1,
                        Username = "administrador",
                        NombreCompleto = "Administrador Principal",
                        Email = "",
                        ComunidadId = 1,
                        RolId = 3,
                        PasswordHash = "AQAAAAIAAYagAAAAEJMWmqGEeofwL3f2r0uCFpykRHUwRHd2S3axzTA2Ox0AVE1hxv7oB/FeWzQJcPb/aA=="
                    },
                    new UsuarioEntity
                    {
                        Id = 2,
                        Username = "admin",
                        NombreCompleto = "Administrador",
                        Email = "",
                        ComunidadId = 1,
                        RolId = 2,
                        PasswordHash = "AQAAAAIAAYagAAAAEJMWmqGEeofwL3f2r0uCFpykRHUwRHd2S3axzTA2Ox0AVE1hxv7oB/FeWzQJcPb/aA=="
                    }
                };

                entity.HasData(usuarios);

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.NombreCompleto).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.Property(e => e.RolId).IsRequired().HasDefaultValue(1);
                entity.Property(e => e.ComunidadId).IsRequired().HasDefaultValue(1);

                entity.HasOne(e => e.Comunidad)
                      .WithMany(e => e.Usuarios)
                      .HasForeignKey(e => e.ComunidadId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Rol)
                      .WithMany()
                      .HasForeignKey(e => e.RolId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ConceptoEntity>(entity =>
            {
                entity.HasData(
                    new ConceptoEntity { Id = 1, Descripcion = "Mantenimiento" },
                    new ConceptoEntity { Id = 2, Descripcion = "Proyecto Extraordinario" }
                    );

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<RolEntity>(entity =>
            {
                entity.HasData(
                    new RolEntity { Id = 1, Descripcion = "Usuario" },
                    new RolEntity { Id = 2, Descripcion = "Administrador" },
                    new RolEntity { Id = 3, Descripcion = "Super Administrador" }
                    );
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(50);
                entity.HasMany<UsuarioEntity>()
                      .WithOne(e => e.Rol)
                      .HasForeignKey(e => e.RolId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IncidenciaEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).IsRequired();
                entity.Property(e => e.EstatusId).IsRequired();
                entity.HasOne(e => e.Usuario)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Usuario)
                      .WithMany(e => e.Incidencias)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.Estatus)
                      .WithMany(e => e.Incidencias)
                      .HasForeignKey(e => e.EstatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<IncidenciaEstatusEntity>(entity =>
            {
                entity.HasData(
                    new IncidenciaEstatusEntity { Id = 1, Descripcion = "Pendiente" },
                    new IncidenciaEstatusEntity { Id = 2, Descripcion = "En Proceso" },
                    new IncidenciaEstatusEntity { Id = 3, Descripcion = "Cerrada" }
                    );
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<ReporteEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(500);
                entity.Property(e => e.FechaRegistro).IsRequired();
                entity.Property(e => e.EstatusId).IsRequired();

                entity.HasOne(e => e.Usuario)
                      .WithMany(e => e.Reportes)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Estatus)
                      .WithMany(e => e.Reportes)
                      .HasForeignKey(e => e.EstatusId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ReporteEstatusEntity>(entity =>
            {
                entity.HasData(
                    new ReporteEstatusEntity { Id = 1, Descripcion = "Pendiente" },
                    new ReporteEstatusEntity { Id = 2, Descripcion = "En Proceso" },
                    new ReporteEstatusEntity { Id = 3, Descripcion = "Resuelto" }
                    );
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(50);
            });
        }
    }
}
