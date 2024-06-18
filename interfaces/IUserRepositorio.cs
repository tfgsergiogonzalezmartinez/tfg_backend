using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto.UserDto;
using backend_tfg.modelos.usuario;

namespace backend_tfg.interfaces
{
    public interface IUserRepositorio : IBaseRepositorio<User>
    {

        Task<RItem<UserLoginGetDto>> Register(UserCreateDto usuarioCreaDto);
        Task<RItem<UserLoginGetDto>> Login(UserLoginDTO usuarioLoginDTO);
        Task<RItem<User>> CambiarContraseña(User usuario);

    }
}