using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.modelos.EntidadChat;
using MongoDB.Driver;

namespace backend_tfg.repositorios
{
    public class ChatRepositorio : BaseRepositorio<Chat>, IChatRepositorio
    {
        private readonly IMongoCollection<Chat> _chatCollection;
        private IConfiguration _config;

        public ChatRepositorio(IConfiguration config, ContextoDB contexto) : base(contexto)
        {
            this._chatCollection = contexto.GetCollection<Chat>();
            this._config = config;
        }


        public async Task<RLista<Chat>> getByUser(string userId)
        {
            var filter = Builders<Chat>.Filter.Eq("UserIds", userId);
            var datos = await _chatCollection.Find(filter).ToListAsync();
            if (datos is null){
                return new RLista<Chat>(null){
                    Mensaje = "No se ha encontrado ningun chat con ese usuario",
                    Resultado = -1
                };
            }
            return new RLista<Chat>(datos);
        }

        public async Task<RItem<Chat>> getByUsers(List<string> userIds)
        {
            var filter = Builders<Chat>.Filter.And(
            Builders<Chat>.Filter.All("UserIds", userIds),
            Builders<Chat>.Filter.Size("UserIds", 2)
            );
        
            var datos = await _chatCollection.Find(filter).FirstOrDefaultAsync();
            if (datos is null){
                return new RItem<Chat>(null){
                    Mensaje = "No se ha encontrado ningun chat con esos usuarios",
                    Resultado = -1
                };
            }
            return new RItem<Chat>(datos);
        }

        public async Task<RItem<Chat>> postMessageUsers ( NewMessage newMsg)
        {

            var chat = await this.getByUsers(new List<string>{newMsg.usuario, newMsg.destinatario});
            if (chat.Resultado != 0){
                var creacion = await this.Create(new Chat{
                    UserIds = new List<string>{newMsg.usuario, newMsg.destinatario},
                    Mensajes = new List<Message>{
                        new Message{
                            UserId = newMsg.usuario,
                            Msg = newMsg.mensaje,
                            Fecha = DateTime.Now
                        }
                    }
                });
                if (creacion.Resultado != 0){
                    return new RItem<Chat>(null){
                        Mensaje = "No se ha podido crear el chat",
                        Resultado = -1
                    };
                }
                return creacion;
            }

            var datos = this._chatCollection.UpdateOne(
                Builders<Chat>.Filter.Eq("Id", chat.Valor.Id),
                Builders<Chat>.Update.Push("Mensajes", new Message{
                    UserId = newMsg.usuario,
                    Msg = newMsg.mensaje,
                    Fecha = DateTime.Now
                })
            );
            var chatActualizado = await this.getByUsers(new List<string>{newMsg.usuario, newMsg.destinatario});
            return chatActualizado;
        }
    }
}