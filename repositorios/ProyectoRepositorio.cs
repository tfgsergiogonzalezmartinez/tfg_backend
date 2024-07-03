using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg;
using backend_tfg.interfaces;
using backend_tfg.repositorios;
using MongoDB.Driver;
using tfg_backend.interfaces;
using tfg_backend.modelos.Proyecto;

namespace tfg_backend.repositorios
{
    public class ProyectoRepositorio : BaseRepositorio<Proyecto>, IProyectoRepositorio
    {
        private IConfiguration _config;
        public ProyectoRepositorio(ContextoDB contexto, IConfiguration config) : base(contexto)
        {
            _config = config;
        }

        public async Task<RLista<Proyecto>> getProyectosUsuario(string idUsuario)
        {
            var datos = await collection.Find(p => p.Usuario == idUsuario).ToListAsync();

            if (datos is null){
                return new RLista<Proyecto>(null){
                    Mensaje = "No se ha encontrado ningun proyecto con ese usuario",
                    Resultado = 0
                };
            }
            return new RLista<Proyecto>(datos);
        }


    }
}