using System.ComponentModel.DataAnnotations;

namespace N5.Domain
{
    public class TipoPermiso
    {
        [Key]
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }
}