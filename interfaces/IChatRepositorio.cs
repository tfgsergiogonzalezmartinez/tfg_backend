using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.EntidadChat;

namespace backend_tfg.interfaces
{
    public interface IChatRepositorio : IBaseRepositorio<Chat>
    {
        Task<RItem<Chat>> getByUsers(List<string> userIds);
        Task<RLista<Chat>> getByUser(string userId);
        Task<RItem<Chat>> postMessageUsers (NewMessage newMsg);
        Task<RItem<Chat>> LeerChat(string idUser1, string idUser2);
        Task<RItem<int>> GetNumMensajesSinLeer(string idUser1, string idUser2);
    }
}