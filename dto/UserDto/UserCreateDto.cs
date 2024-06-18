using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.usuario;

namespace backend_tfg.dto.UserDto
{

    public class UserCreateDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }

        public UserCreateDto()
        {
        }
        public UserCreateDto(User usuario)
        {
            Email = usuario.Email;
            Password = usuario.Password;
            Nombre = usuario.Nombre;
            Apellido1 = usuario.Apellido1;
            Apellido2 = usuario.Apellido2;
        }

        public void toEntidad(User usuario)
        {
            usuario.Email = Email;
            usuario.Password = Password;
            usuario.Nombre = Nombre;
            usuario.Apellido1 = Apellido1;
            usuario.Apellido2 = Apellido2;
        }


    }
}
