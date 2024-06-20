using System.ComponentModel.DataAnnotations;

namespace N5.Domain
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public int TipoPermisoId { get; set; }
        public DateTime FechaPermiso { get; set; }
        public TipoPermiso TipoPermiso { get; set; }
    }
}