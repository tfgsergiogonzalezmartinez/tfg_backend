using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace backend_tfg.dto.UserDto

{
    public class UserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }


        public UserLoginDto()
        {
        }
    }
}