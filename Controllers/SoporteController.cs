using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tfg_backend.dto.SoporteDto;
using tfg_backend.interfaces;


namespace backend_tfg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SoporteController : ControllerBase
    {
        private readonly ISoporteRepositorio _soporteRepositorio;

        public SoporteController(ISoporteRepositorio soporteRepositorio)
        {
            this._soporteRepositorio = soporteRepositorio;
        }

        [HttpGet("GetPeticionByUsuario/{usuarioId}")]
        public async Task<ActionResult<List<PeticionSoporte>>> GetPeticionByUsuario(string usuarioId)
        {
            var datos = await _soporteRepositorio.GetPeticionByUsuario(usuarioId);
            return Ok(datos.Lista);
        }

        [HttpGet("GetPeticionesCerradas/{usuarioId}")]
        public async Task<ActionResult<List<PeticionSoporte>>> GetPeticionesCerradas(string usuarioId)
        {
            var datos = await _soporteRepositorio.GetPeticionesCerradas();
            return Ok(datos.Lista);
        }

        [HttpGet("GetPeticionesAbiertas/{usuarioId}")]
        public async Task<ActionResult<List<PeticionSoporte>>> GetPeticionesAbiertas(string usuarioId)
        {
            var datos = await _soporteRepositorio.GetPeticionesAbiertas();
            return Ok(datos.Lista);
        }

        [HttpPost("AbrirPeticion")]
        public async Task<ActionResult<PeticionSoporte>> AbrirPeticion(string peticionId)
        {
            var dato = await _soporteRepositorio.AbrirPeticion(peticionId);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }

        [HttpPost("CerrarPeticion")]
        public async Task<ActionResult<PeticionSoporte>> CerrarPeticion(string peticionId)
        {
            var dato = await _soporteRepositorio.CerrarPeticion(peticionId);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }

        [HttpPost("AsignarPeticionAdmin")]
        public async Task<ActionResult<PeticionSoporte>> AsignarPeticionAdmin(SoporteAsignarPeticionDto soporteAsignarPeticionDto)
        {
            var dato = await _soporteRepositorio.AsignarPeticionAdmin(soporteAsignarPeticionDto.IdPeticion, soporteAsignarPeticionDto.IdUsuarioAdmin);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }
    }




}
