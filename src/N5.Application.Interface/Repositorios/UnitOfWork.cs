using N5.Application.Servicios.Interfaces;
using N5.Domain;
using N5.Persistence;

namespace N5.Application.Servicios.Repositorios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Permiso> _permisoRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Permiso> PermisoRepository
        {
            get { return _permisoRepository ??= new Repository<Permiso>(_context); }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}