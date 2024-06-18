using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.EntidadChat;

namespace backend_tfg.interfaces
{
    public interface IChatRepositorio : IBaseRepositorio<Chat>
    {
        Task<RLista<Chat>> getByUsers(List<string> userIds);
    }
}