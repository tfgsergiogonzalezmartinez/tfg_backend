using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_tfg.dto.ChatDto
{
    public class ChatGetDto : EntidadGetDto
    {
        public List<string> UserIds { get; set; }  // Listas de dos en dos, o posibilidad de crear grupos si hay mas de dos
        public List<MessageDto> Mensajes { get; set; }

        public ChatGetDto() : base()
        {
            
        }
    }
}