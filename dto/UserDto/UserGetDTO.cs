using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using backend_tfg.modelos.usuario;
using MongoDB.Bson.Serialization.Serializers;

namespace backend_tfg.dto.UserDto
{
    public class UserGetDto : EntidadGetDto
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public string Rol { get; set; }
        public DateTime FechaNacimiento { get; set; }
        
        public UserGetDto() : base()
        {

        }
        public UserGetDto(User usuario) : base(usuario)
        {
            Email = usuario.Email;
            Nombre = usuario.Nombre;
            Apellido1 = usuario.Apellido1;
            Apellido2 = usuario.Apellido2;
            Rol = usuario.Rol;
            FechaNacimiento = usuario.FechaNacimiento;
        }
        public static List<UserGetDto> convListaDto(List<User> listaUsers){
            List<UserGetDto> listaDto = new List<UserGetDto>();
            foreach (User user in listaUsers){
                listaDto.Add(new UserGetDto(user));
            }
            return listaDto;
        }
    }
    
}