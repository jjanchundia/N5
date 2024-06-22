namespace N5.Application.Servicios.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task Crear(T entidad);
        Task Modificar(T entidad);
        Task<IEnumerable<T>> ObtenerTodo();
        Task<T> ObtenerPorId(int id);
    }
}