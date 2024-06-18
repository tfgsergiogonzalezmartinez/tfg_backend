using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto.ChatDto;
using backend_tfg.interfaces;
using backend_tfg.modelos.EntidadChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_tfg.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepositorio _chatRepositorio;
        public ChatController(IChatRepositorio chatRepositorio)
        {
            this._chatRepositorio = chatRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<Chat>>> GetAll()
        {   
            var datos = await _chatRepositorio.GetAll();
            return Ok(datos.Lista);
        }
        [HttpGet("GetByUsers{userName}")]
        public async Task<ActionResult<List<Chat>>> GetByUser(string userName)
        {
            var dato = await _chatRepositorio.getByUsers(new List<string> {userName});
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Lista);

        }
        [HttpPost]
        public async Task<ActionResult<List<Chat>>> Create(ChatCreateDto chat)
        {
            if (chat.UserIds.Count < 2){
                return BadRequest("Se necesita al menos dos usuarios para crear un chat");
            }
            var buscar = await _chatRepositorio.getByUsers(chat.UserIds);
            if (buscar.Resultado == 0)
            {
                return BadRequest("Ya existe un chat con esos usuarios");
            }   
            var dato = await _chatRepositorio.Create(chat.toEntidad(new Chat()));
            return Ok(dato.Valor);
        }
    }
}
