using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.usuario;

namespace backend_tfg.dto.UserDto
{
    public class UserLoginGetDto : UserGetDto
    {
        public string Token { get; set; }
   
        public UserLoginGetDto() : base()
        {
        }
        public UserLoginGetDto(User usuario) : base(usuario)
        {
            
        }
    }

}