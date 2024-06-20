using Microsoft.EntityFrameworkCore;
using N5.Domain;

namespace N5.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Permiso> Permiso { get; set; }
        public DbSet<TipoPermiso> TipoPermiso { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Permiso>()
                .HasOne(p => p.TipoPermiso)
                .WithMany(tp => tp.Permisos)
                .HasForeignKey(p => p.TipoPermisoId);
        }
    }
}