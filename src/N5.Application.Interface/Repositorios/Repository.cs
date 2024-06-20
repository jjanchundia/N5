using Microsoft.EntityFrameworkCore;
using N5.Application.Servicios.Interfaces;

namespace N5.Application.Servicios.Repositorios
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task SolicitarPermiso(T entidad)
        {
            await _dbSet.AddAsync(entidad);
        }

        public async Task ModificarPermiso(T entidad)
        {
            _dbSet.Update(entidad);
        }

        public async Task<IEnumerable<T>> ObtenerPermisos()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> ObtenerPermisoPorId(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}