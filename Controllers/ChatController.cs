using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        [HttpGet("GetByUsers/{userId1}/{userId2}")]
        public async Task<ActionResult<List<Chat>>> GetByUsers(string userId1, string userId2) //es una lista por si mas adelante quiero hacer chats de mas de 2 personas
        {
            var dato = await _chatRepositorio.getByUsers([userId1, userId2]);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }

        [HttpPost("LeerChat")]
        public async Task<ActionResult<List<Chat>>> LeerChat(ChatUsuariosRequestDto chatUsuariosRequest)
        {
            var dato = await _chatRepositorio.LeerChat(chatUsuariosRequest.UserId1, chatUsuariosRequest.UserId2);
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
        
        [HttpGet("GetChatsAbiertos/{userId}")]
        public async Task<ActionResult<List<Chat>>> GetChatsAbiertos(string userId)
        {
            var dato = await _chatRepositorio.GetChatsAbiertos(userId);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Lista);
        }

        [HttpPost("CerrarChat")]
        public async Task<ActionResult<List<Chat>>> CerrarChat(ChatUsuariosRequestDto chatUsuariosRequest)
        {
            var dato = await _chatRepositorio.CerrarChat(chatUsuariosRequest.UserId1, chatUsuariosRequest.UserId2);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
        }

        [HttpPost("AbrirChat")]
        public async Task<ActionResult<List<Chat>>> AbrirChat(ChatUsuariosRequestDto chatUsuariosRequest)
        {
            var dato = await _chatRepositorio.Abrirchat(chatUsuariosRequest.UserId1, chatUsuariosRequest.UserId2);
            if (dato.Resultado != 0)
            {
                return BadRequest(dato.Mensaje);
            }
            return Ok(dato.Valor);
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
