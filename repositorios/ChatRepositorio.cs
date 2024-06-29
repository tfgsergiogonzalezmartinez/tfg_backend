using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.modelos.EntidadChat;
using MongoDB.Bson;
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
                    Abierto = true,
                    Mensajes = new List<Message>{
                        new Message{
                            UserId = newMsg.usuario,
                            Msg = newMsg.mensaje,
                            Fecha = DateTime.Now,
                            Leido = false
                            
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
                    Fecha = DateTime.Now,
                    Leido = false
                })
            );
            var chatActualizado = await this.getByUsers(new List<string>{newMsg.usuario, newMsg.destinatario});
            return chatActualizado;
        }

        public async Task<RItem<Chat>> LeerChat(string idUser1, string idUser2)
        {
            var chat = await this.getByUsers(new List<string>{idUser1, idUser2});
            if (chat.Resultado != 0){
                return new RItem<Chat>(null){
                    Mensaje = "No se ha encontrado el chat",
                    Resultado = -1
                };
            }
            var chatActualizado = this._chatCollection.UpdateOne(
                Builders<Chat>.Filter.Eq("Id", chat.Valor.Id),
                Builders<Chat>.Update.Set("Mensajes.$[msg].Leido", true),
                new UpdateOptions{
                    ArrayFilters = new List<ArrayFilterDefinition>{
                        new BsonDocumentArrayFilterDefinition<BsonDocument>(new BsonDocument("msg.UserId", idUser1))
                    }
                }
            );
            return await this.GetById(chat.Valor.Id);
   
        }

        public async Task<RItem<int>> GetNumMensajesSinLeer(string idUser1, string idUser2)
        {
            var filter = new List<BsonDocument>
            {
                new BsonDocument("$match", 
                new BsonDocument("UserIds", 
                new BsonDocument
                        {
                            { "$all", 
                new BsonArray
                            {
                                idUser1,
                                idUser2
                            } }, 
                            { "$size", 2 }
                        })),
                new BsonDocument("$unwind", "$Mensajes"),
                new BsonDocument("$match", 
                new BsonDocument
                    {
                        { "Mensajes.Leido", false }, 
                        { "Mensajes.UserId", idUser1 }
                    }),
                new BsonDocument("$count", "MensajesNoLeidos")
            };
            var datos = await _chatCollection.Aggregate<BsonDocument>(filter).FirstOrDefaultAsync();
            
            if (datos is null){
                return new RItem<int>(0){
                    Mensaje = "No se ha encontrado ningun chat con esos usuarios",
                    Resultado = 0
                };
            }

            return new RItem<int>(datos["MensajesNoLeidos"].ToInt32());
        }


        public async Task<RLista<Chat>> GetChatsAbiertos(string userId)
        {
            var filter = new List<BsonDocument>
            {
                new BsonDocument("$match", 
                new BsonDocument
                    {
                        { "UserIds", userId }, 
                        { "Abierto", true }
                    })
            };
            var datos = await _chatCollection.Aggregate<Chat>(filter).ToListAsync();
            if (datos is null){
                return new RLista<Chat>(null){
                    Mensaje = "No se ha encontrado ningun chat con ese usuario",
                    Resultado = 0
                };
            }
            return new RLista<Chat>(datos);
        
        }
        public async Task<RItem<Chat>> CerrarChat(string userId1, string userId2)
        {
            var filter = Builders<Chat>.Filter.And(
                Builders<Chat>.Filter.All("UserIds", new List<string> { userId1, userId2 }),
                Builders<Chat>.Filter.Size("UserIds", 2)
            );

            var update = Builders<Chat>.Update.Set("Abierto", false);

            var result = await collection.UpdateManyAsync(filter, update);
            if (result.ModifiedCount == 0){
                return new RItem<Chat>(null){
                    Mensaje = "No se ha modificado ningun chat con esos usuarios",
                    Resultado = 0
                };
            }
            return new RItem<Chat>(null);
        }
    
        public async Task<RItem<Chat>> Abrirchat(string userId1, string userId2)
        {
            var filter = Builders<Chat>.Filter.And(
                Builders<Chat>.Filter.All("UserIds", new List<string> { userId1, userId2 }),
                Builders<Chat>.Filter.Size("UserIds", 2)
            );

            var update = Builders<Chat>.Update.Set("Abierto", true);

            var result = await collection.UpdateManyAsync(filter, update);
            if (result.ModifiedCount == 0){
                return new RItem<Chat>(null){
                    Mensaje = "No se ha modificado ningun chat con esos usuarios",
                    Resultado = 0
                };
            }
            return new RItem<Chat>(null);

        }
        

   
    }
}