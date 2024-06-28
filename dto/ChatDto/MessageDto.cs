using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.EntidadChat;

namespace backend_tfg.dto.ChatDto
{
    public class MessageDto
    {
        public string UserId { get; set; }
        public string Msg { get; set; }
        public bool Leido { get; set; }
        public DateTime? Fecha { get; set; }

        public MessageDto()
        {
            
        }
        public MessageDto(Message message)
        {
            UserId = message.UserId;
            Msg = message.Msg;
            Fecha = message.Fecha;
            Leido = message.Leido;
        }
        public Message toEntidad(Message entidad){
            entidad.UserId = UserId;
            entidad.Msg = Msg;
            entidad.Fecha = Fecha;
            entidad.Leido = Leido;
            return entidad;
        }
    }
}