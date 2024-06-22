using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto.UserDto;
using backend_tfg.modelos.usuario;
using tfg_backend.dto.UserDto;

namespace backend_tfg.interfaces
{
    public interface IUserRepositorio : IBaseRepositorio<User>
    {

        Task<RItem<UserLoginGetDto>> Register(UserCreateDto usuarioCreaDto);
        Task<RItem<UserLoginGetDto>> Login(UserLoginDto usuarioLoginDto);
        Task<RItem<User>> CambiarPassword(UserCambiarPasswordDto userCambiarPassword);
        Task<RItem<User>> ModificarRol(UserCambiarRolDto userModificarRolDto);

    }
}