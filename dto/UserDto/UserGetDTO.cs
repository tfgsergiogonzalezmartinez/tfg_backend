using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.usuario;
using MongoDB.Bson.Serialization.Serializers;

namespace backend_tfg.dto.UserDto
{
    public class UserGetDTO : EntidadGetDto
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Rol { get; set; }
        public DateTime FechaNacimiento { get; set; }
        
        public UserGetDTO() : base()
        {

        }
        public UserGetDTO(User usuario) : base(usuario)
        {
            Email = usuario.Email;
            Nombre = usuario.Nombre;
            Apellido1 = usuario.Apellido1;
            Apellido2 = usuario.Apellido2;
            Rol = usuario.Rol;
            FechaNacimiento = usuario.FechaNacimiento;
        }
    }
    
}