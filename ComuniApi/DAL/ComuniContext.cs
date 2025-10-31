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
                        Correo = ""
                    });

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Direccion).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.MaxUsers).IsRequired();
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

                entity.HasOne(e => e.Usuario)
                      .WithMany(e => e.EdoCuentas)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<UsuarioEntity>(entity =>
            {
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
                    new RolEntity { Id = 2, Descripcion = "Administrador" }
                    );
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(50);
                entity.HasMany<UsuarioEntity>()
                      .WithOne(e => e.Rol)
                      .HasForeignKey(e => e.RolId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
