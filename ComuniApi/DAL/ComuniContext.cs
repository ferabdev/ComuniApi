using Microsoft.EntityFrameworkCore;
using System;

namespace ComuniApi.DAL
{
    public class ComuniContext : DbContext
    {
        public ComuniContext(DbContextOptions<ComuniContext> options) : base(options) { }

        // Ejemplo de tablas
        public DbSet<UsuarioEntity> Usuarios { get; set; }

    }
}
