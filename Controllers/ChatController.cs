using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto.ChatDto;
using backend_tfg.interfaces;
using backend_tfg.modelos.EntidadChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tfg_backend.dto.ChatDto;

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

        [HttpGet("GetByUser/{idUser}")]
        public async Task<ActionResult<List<Chat>>> GetByUser(string idUser)
        {
            var dato = await _chatRepositorio.getByUser(idUser);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Lista);
        }

        [HttpGet("GetByUsers/{idsUsers}")]
        public async Task<ActionResult<List<Chat>>> GetByUser(List<string> idsUsers)
        {
            var dato = await _chatRepositorio.getByUsers(idsUsers);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }

        [HttpGet("LeerChat/{idUser1}/{idUser2}")]
        public async Task<ActionResult<List<Chat>>> LeerChat(string idUser1,string idUser2)
        {
            var dato = await _chatRepositorio.LeerChat(idUser1, idUser2);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }

        [HttpGet("GetNumMensajesSinLeer/{idUser1}/{idUser2}")]
        public async Task<ActionResult<List<Chat>>> GetNumMensajesSinLeer(string idUser1, string idUser2)
        {
            var dato = await _chatRepositorio.GetNumMensajesSinLeer(idUser1, idUser2);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            
            return Ok(new MensajesNoLeidosDto{
                MensajesNoLeidos = dato.Valor
            });
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
