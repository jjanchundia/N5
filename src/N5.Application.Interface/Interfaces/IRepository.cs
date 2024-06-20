namespace N5.Application.Servicios.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task SolicitarPermiso(T entidad);
        Task ModificarPermiso(T entidad);
        Task<IEnumerable<T>> ObtenerPermisos();
        Task<T> ObtenerPermisoPorId(int id);
    }
}