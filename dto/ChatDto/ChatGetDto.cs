using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_tfg.dto.ChatDto
{
    public class ChatGetDto : EntidadGetDto
    {
        public List<string> UserIds { get; set; } //El grupo del WS sera el id de ambos usuarios
        public List<MessageDto> Messages { get; set; }

        public ChatGetDto() : base()
        {
            
        }
    }
}