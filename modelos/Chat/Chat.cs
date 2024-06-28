using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.models;

namespace backend_tfg.modelos.EntidadChat
{
    public class Chat : Entidad
    {
        public List<string> UserIds { get; set; } = new List<string>();    // Listas de dos en dos, o posibilidad de crear grupos si hay mas de dos
        public List<Message> Mensajes { get; set; } = new List<Message>();
    }
}