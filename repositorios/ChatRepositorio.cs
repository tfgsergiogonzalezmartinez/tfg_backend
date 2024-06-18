using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.modelos.EntidadChat;
using MongoDB.Driver;

namespace backend_tfg.repositorios
{
    public class ChatRepositorio : BaseRepositorio<Chat>, IChatRepositorio
    {
        private readonly IMongoCollection<Chat> _usuariosCollection;
        private IConfiguration _config;

        public ChatRepositorio(IConfiguration config, ContextoDB contexto) : base(contexto)
        {
            this._usuariosCollection = contexto.GetCollection<Chat>();
            this._config = config;
        }



        public async Task<RLista<Chat>> getByUsers(List<string> userIds)
        {
            var filter = Builders<Chat>.Filter.All("UserIds", userIds);
            var datos = await _usuariosCollection.Find(filter).ToListAsync();
            if (datos is null){
                return new RLista<Chat>(null){
                    Mensaje = "No se ha encontrado ningun chat con esos usuarios",
                    Resultado = -1
                };
            }

            return new RLista<Chat>(datos);

        }
    }
}