﻿using System.ComponentModel.DataAnnotations;

namespace N5.Domain
{
    public class Permiso
    {
        [Key]
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string ApellidoEmpleado { get; set; }
        public int TipoPermiso { get; set; }
        public DateTime FechaPermiso { get; set; }
        public TipoPermiso TipoPermisos { get; set; }
    }
}