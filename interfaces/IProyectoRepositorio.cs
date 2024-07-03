using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using tfg_backend.modelos.Proyecto;

namespace tfg_backend.interfaces
{
    public interface IProyectoRepositorio : IBaseRepositorio<Proyecto>
    {
        Task<RLista<Proyecto>> getProyectosUsuario(string idUsuario);
    }
}