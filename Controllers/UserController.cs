using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto.UserDto;
using backend_tfg.interfaces;
using backend_tfg.modelos.usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_tfg.Controllers
 {
    
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepositorio _usuarioRepositorio;

        public UserController(IUserRepositorio usuarioRepositorio)
        {
            this._usuarioRepositorio = usuarioRepositorio;
        }
        
        [Authorize(Roles ="admin")]
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAll()
        {
            var datos = await _usuarioRepositorio.GetAll();
            return Ok(datos.Lista);
        }

        [Authorize(Roles ="admin")]   
        [HttpPost]
        public async Task<ActionResult<User>> crearUsuario(User usuario)
        {
            var dato = await _usuarioRepositorio.Create(usuario);

            if (dato.Resultado != 0){
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato);
        }


        
        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserLoginDTO usuarioLoginDTO)
        {
            var dato = await _usuarioRepositorio.Login(usuarioLoginDTO);
            if (dato.Resultado != 0){
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
            
        }
    }
}