using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto.UserDto;
using backend_tfg.interfaces;
using backend_tfg.modelos.usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tfg_backend.dto.UserDto;

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

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<List<UserGetDto>>> GetAll()
        {
            var datos = await _usuarioRepositorio.GetAll();
            if (datos.Lista is null){
                return BadRequest(datos.Mensaje);
            }
                return Ok(UserGetDto.convListaDto(datos.Lista));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult<User>> crearUsuario(User usuario)
        {
            var dato = await _usuarioRepositorio.Create(usuario);

            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserLoginGetDto>> Register(UserCreateDto usuarioCreaDTO)
        {
            var dato = await _usuarioRepositorio.Register(usuarioCreaDTO);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserGetDto>> Login(UserLoginDto usuarioLoginDTO)
        {
            var dato = await _usuarioRepositorio.Login(usuarioLoginDTO);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);

        }
        [HttpPost("CambiarPassword")]
        public async Task<ActionResult<UserGetDto>> CambiarPassword(UserCambiarPasswordDto userCambiarPassword)
        {
            var dato = await _usuarioRepositorio.CambiarPassword(userCambiarPassword);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }

            return Ok(new UserGetDto(dato.Valor));

        }
        [HttpPost("ModificarRol")]
        public async Task<ActionResult<UserGetDto>> ModificarRol(UserCambiarRolDto userCambiarRolDto)
        {
            var dato = await _usuarioRepositorio.ModificarRol(userCambiarRolDto);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }

            return Ok(new UserGetDto(dato.Valor));
        }

        [HttpGet("ObtenerUsuariosCoincidentes/{nombre}")]
        public async Task<ActionResult<UserGetDto>> ObtenerUsuariosCoincidentes(string nombre)
        {
            var dato = await _usuarioRepositorio.ObtenerUsuariosCoincidentes(nombre);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(UserGetDto.convListaDto(dato.Lista));
        }

        [HttpPost("{userId}/subirBase64")]
        public async Task<IActionResult> SubirFotoBase64(string userId, [FromBody] ImagenDto imagenDto)
        {
            if (string.IsNullOrEmpty(imagenDto.Imagen))
                return BadRequest("No se subió ninguna imagen.");

            var dataUriHeader = imagenDto.Imagen.Split(',')[0];
            var base64Data = imagenDto.Imagen.Split(',')[1];
            string extension;

            if (dataUriHeader.Contains("image/jpeg"))
            {
                extension = ".jpg";
            }
            else if (dataUriHeader.Contains("image/png"))
            {
                extension = ".png";
            }
            else if (dataUriHeader.Contains("image/gif"))
            {
                extension = ".gif";
            }
            else
            {
                return BadRequest("Formato de imagen no soportado.");
            }

            var rutaCarpeta = Path.Combine("data", userId);
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            var validExtensions = new[] { ".jpg", ".png", ".gif" };
            foreach (var ext in validExtensions)
            {
                var existingAvatarPath = Path.Combine(rutaCarpeta, "avatar" + ext);
                if (System.IO.File.Exists(existingAvatarPath))
                {
                    System.IO.File.Delete(existingAvatarPath);
                }
            }

            var rutaArchivo = Path.Combine(rutaCarpeta, "avatar" + extension);
            var imageData = Convert.FromBase64String(base64Data);
            await System.IO.File.WriteAllBytesAsync(rutaArchivo, imageData);

            return Ok(new { RutaArchivo = rutaArchivo });
        }
        
        [HttpGet("{userId}/fotoBase64")]
        public async Task<IActionResult> ObtenerFotoBase64(string userId)
        {
            var rutaCarpeta = Path.Combine("data", userId);
            var rutaArchivoJpg = Path.Combine(rutaCarpeta, "avatar.jpg");
            var rutaArchivoJpeg = Path.Combine(rutaCarpeta, "avatar.jpeg");
            var rutaArchivoPng = Path.Combine(rutaCarpeta, "avatar.png");
            var rutaArchivoGif = Path.Combine(rutaCarpeta, "avatar.gif");

            string rutaArchivo = null;
            if (System.IO.File.Exists(rutaArchivoJpg))
                rutaArchivo = rutaArchivoJpg;
            else if (System.IO.File.Exists(rutaArchivoJpeg))
                rutaArchivo = rutaArchivoJpeg;
            else if (System.IO.File.Exists(rutaArchivoPng))
                rutaArchivo = rutaArchivoPng;
            else if (System.IO.File.Exists(rutaArchivoGif))
                rutaArchivo = rutaArchivoGif;

            if (rutaArchivo == null)
            {
                return NotFound("Archivo no encontrado.");
            }

            var imageData = System.IO.File.ReadAllBytes(rutaArchivo);
            var base64Imagen = $"data:image/{Path.GetExtension(rutaArchivo).TrimStart('.')};base64,{Convert.ToBase64String(imageData)}";

            return Ok(new ImagenDto { Imagen = base64Imagen });
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<UserGetDto>> Get(string userId)
        {
            var dato = await _usuarioRepositorio.GetById(userId);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }

            return Ok(new UserGetDto(dato.Valor));
        }
    }

    
}