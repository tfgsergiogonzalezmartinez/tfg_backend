using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using backend_tfg.modelos.EntidadChat;

namespace backend_tfg.dto.ChatDto
{
    public class ChatCreateDto
    {
        public List<string> UserIds { get; set; }  // Listas de dos en dos, o posibilidad de crear grupos si hay mas de dos
        public List<MessageDto> Messages { get; set; }

        public ChatCreateDto()
        {
            UserIds = new List<string>();
            Messages = new List<MessageDto>();
        }

        public Chat toEntidad(Chat entidad ){
            entidad.UserIds = UserIds;
            entidad.Messages = new List<Message>();
            foreach (var msg in this.Messages)
            {
                entidad.Messages.Add(msg.toEntidad(new Message()));
            }
            return entidad;
        }

    }


}