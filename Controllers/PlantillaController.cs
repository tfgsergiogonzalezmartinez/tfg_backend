using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tfg_backend.interfaces;
using tfg_backend.modelos.Plantilla;

namespace tfg_backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PlantillaController : ControllerBase
    {

        public readonly IPlantillaRepositorio _plantillaRepositorio;

        public PlantillaController(IPlantillaRepositorio plantillaRepositorio)
        {
            this._plantillaRepositorio = plantillaRepositorio;
        }

        [HttpGet()]
        public async Task<ActionResult<List<Plantilla>>> GetAll()
        {
            var datos = await _plantillaRepositorio.GetAll();
            if (datos.Resultado != 0)
            {
                return BadRequest(datos.Mensaje);
            }
            return Ok(datos.Lista);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RItem<Plantilla>>> GetById(string id){
            var datos = await _plantillaRepositorio.GetById(id);
            if (datos.Resultado != 0)
            {
                return BadRequest(datos.Mensaje);
            }
            return Ok(datos.Valor);
        }
        
    }
}