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
        public DbSet<UsuarioEntity> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
            });

            modelBuilder.Entity<ConceptoEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<ConceptoEntity>().HasData(
                new ConceptoEntity { Id = 1, Descripcion = "Mantenimiento" },
                new ConceptoEntity { Id = 2, Descripcion = "Proyecto Extraordinario" }
            );
        }
    }
}
