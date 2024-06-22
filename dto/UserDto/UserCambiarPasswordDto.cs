using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tfg_backend.dto.UserDto
{
    public class UserCambiarPasswordDto
    {
        public string Email { get; set; } = "";
        public string PasswordAntigua { get; set; } = "";
        public string PasswordNueva1 { get; set; } = "";
        public string PasswordNueva2 { get; set; } = "";
    }
}