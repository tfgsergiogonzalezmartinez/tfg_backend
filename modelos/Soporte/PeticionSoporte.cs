using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.EntidadChat;
using backend_tfg.models;

public class PeticionSoporte : Entidad
{
    public string UsuarioPeticionario { get; set; } = "";
    public string SolucionadoByAdmin { get; set; } = "";
    public Boolean Abierta { get; set; } = true;
    public List<Message> Mensajes { get; set; } = new List<Message>();
}
