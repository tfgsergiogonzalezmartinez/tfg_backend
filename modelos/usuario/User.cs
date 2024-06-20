using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.models;

namespace backend_tfg.modelos.usuario
{
    public class User : Entidad
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido1 { get; set; } = string.Empty;
        public string Apellido2 { get; set; } = string.Empty;
        public DateTime FechaNacimiento { get; set; } = DateTime.Now;
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool Listable { get; set; } = false;
    }
}