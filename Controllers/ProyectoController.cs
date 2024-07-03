using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using backend_tfg;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tfg_backend.interfaces;
using tfg_backend.modelos.Proyecto;

namespace tfg_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProyectoController : ControllerBase
    {
        private readonly IProyectoRepositorio _proyectoRepositorio;
        public ProyectoController(ContextoDB contextoDb, IProyectoRepositorio proyectoRepositorio)
        {
            this._proyectoRepositorio = proyectoRepositorio;
        }


        [HttpGet()]
        public async Task<ActionResult<List<Proyecto>>> GetAll()
        {
            var datos = await _proyectoRepositorio.GetAll();
            if (datos.Resultado != 0)
            {
                return BadRequest(datos.Mensaje);
            }
            return Ok(datos.Lista);
        }

        [HttpGet("getProyectosUsuario/{idUsuario}")]
        public async Task<ActionResult<List<Proyecto>>> getProyectosUsuario(string idUsuario)
        {
            var dato = await _proyectoRepositorio.getProyectosUsuario(idUsuario);
            return Ok(dato.Lista);
        }
        
    }
}