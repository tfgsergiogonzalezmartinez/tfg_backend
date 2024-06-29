using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.modelos.EntidadChat;
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



        [HttpGet("{id}")]
        public async Task<ActionResult<RItem<PeticionSoporte>>> GetById(string id)
        {
            var datos = await _soporteRepositorio.GetById(id);
            if (datos is null)
            {
                return BadRequest(datos.Mensaje);
            }
            return Ok(datos.Valor);
        }

        [HttpGet("GetPeticionByUsuario/{usuarioId}")]
        public async Task<ActionResult<List<PeticionSoporte>>> GetPeticionByUsuario(string usuarioId)
        {
            var datos = await _soporteRepositorio.GetPeticionByUsuario(usuarioId);
            return Ok(datos.Lista);
        }

        [HttpGet("GetPeticionesCerradas")]
        public async Task<ActionResult<List<PeticionSoporte>>> GetPeticionesCerradas()
        {
            var datos = await _soporteRepositorio.GetPeticionesCerradas();
            return Ok(datos.Lista);
        }

        [HttpGet("GetPeticionesAbiertas")]
        public async Task<ActionResult<List<PeticionSoporte>>> GetPeticionesAbiertas()
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

        [HttpPost()]
        public async Task<ActionResult<PeticionSoporte>> CrearPeticion(PeticionSoporte soporteCrearPeticionDto)
        {
            var dato = await _soporteRepositorio.Create(soporteCrearPeticionDto);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }
    }




}
