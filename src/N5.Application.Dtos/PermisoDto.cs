
namespace N5.Application.Dtos
{
    public class PermisoDto
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public int TipoPermisoId { get; set; }
        public DateTime FechaPermiso { get; set; }
    }
}