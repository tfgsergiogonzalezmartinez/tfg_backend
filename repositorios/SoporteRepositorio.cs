using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg;
using backend_tfg.interfaces;
using backend_tfg.modelos.EntidadChat;
using backend_tfg.repositorios;
using MongoDB.Driver;
using tfg_backend.interfaces;

namespace tfg_backend.repositorios
{
    public class SoporteRepositorio : BaseRepositorio<PeticionSoporte>, ISoporteRepositorio
    {
        public SoporteRepositorio(ContextoDB contexto) : base(contexto)
        {
        }

        public async Task<RLista<PeticionSoporte>> GetPeticionByUsuario(string usuarioId)
        {
            var filter = Builders<PeticionSoporte>.Filter.Eq("UsuarioPeticionario", usuarioId);
            var datos = await collection.Find(filter).ToListAsync();
            if (datos is null)
            {
                return new RLista<PeticionSoporte>(null)
                {
                    Mensaje = "No se ha encontrado ninguna peticion con ese usuario",
                    Resultado = 0
                };
            }
            return new RLista<PeticionSoporte>(datos);
        }

        public async Task<RItem<PeticionSoporte>> AsignarPeticionAdmin(string idPeticion, string idAdmin)
        {
            var filter = Builders<PeticionSoporte>.Filter.Eq("Id", idPeticion);
            var update = Builders<PeticionSoporte>.Update.Set("SolucionadoByAdmin", idAdmin);
            var dato = await collection.FindOneAndUpdateAsync(filter, update);
            if (dato is null)
            {
                return new RItem<PeticionSoporte>(null)
                {
                    Mensaje = "No se ha encontrado ninguna peticion con ese id",
                    Resultado = -1
                };
            }
            return new RItem<PeticionSoporte>(dato);
        }

        public async Task<RLista<PeticionSoporte>> GetPeticionesAbiertas()
        {
            var filter = Builders<PeticionSoporte>.Filter.Eq("Abierta", true);
            var datos = await collection.Find(filter).ToListAsync();
            if (datos is null)
            {
                return new RLista<PeticionSoporte>(null)
                {
                    Mensaje = "No se ha encontrado ninguna peticion abierta",
                    Resultado = 0
                };
            }
            return new RLista<PeticionSoporte>(datos);
        }

        public async Task<RLista<PeticionSoporte>> GetPeticionesCerradas()
        {
            var filter = Builders<PeticionSoporte>.Filter.Eq("Abierta", false);
            var datos = await collection.Find(filter).ToListAsync();

            if (datos is null)
            {
                return new RLista<PeticionSoporte>(null)
                {
                    Mensaje = "No se ha encontrado ninguna peticion cerrada",
                    Resultado = 0
                };
            }
            return new RLista<PeticionSoporte>(datos);

        }


        public async Task<RItem<PeticionSoporte>> CerrarPeticion(string peticionId, string idUserAdmin)
        {
            var filter = Builders<PeticionSoporte>.Filter.Eq("Id", peticionId);
            var update = Builders<PeticionSoporte>.Update.Set("Abierta", false)
                        .Set("SolucionadoByAdmin", idUserAdmin);
            var dato = await collection.FindOneAndUpdateAsync(filter, update);
            if (dato is null)
            {
                return new RItem<PeticionSoporte>(null)
                {
                    Mensaje = "No se ha encontrado ninguna peticion con ese id",
                    Resultado = -1
                };
            }
            return new RItem<PeticionSoporte>(dato);
        }

        public async Task<RItem<PeticionSoporte>> AbrirPeticion(string peticionId)
        {
            var filter = Builders<PeticionSoporte>.Filter.Eq("Id", peticionId);
            var update = Builders<PeticionSoporte>.Update.Set("Abierta", true);
            var dato = await collection.FindOneAndUpdateAsync(filter, update);
            if (dato is null)
            {
                return new RItem<PeticionSoporte>(null)
                {
                    Mensaje = "No se ha encontrado ninguna peticion con ese id",
                    Resultado = -1
                };
            }
            return new RItem<PeticionSoporte>(dato);
        }

        public async Task<RItem<PeticionSoporte>> PostMessageUsers(string idPeticion ,NewMessage newMsg)
        {

            var datos = this.collection.UpdateOne(
                Builders<PeticionSoporte>.Filter.Eq("Id", idPeticion),
                Builders<PeticionSoporte>.Update.Push("Mensajes", new Message{
                    UserId = newMsg.usuario,
                    Msg = newMsg.mensaje,
                    Fecha = DateTime.Now,
                    Leido = false
                })
            );
            var chatActualizado = await this.collection.Find(peticion => peticion.Id == idPeticion).FirstOrDefaultAsync();
            return new RItem<PeticionSoporte>(chatActualizado);
        }

        public async Task<RItem<PeticionSoporte>> NuevaPeticion(string Asunto ,string Descripcion, string UsuarioId)
        {
            var creacion = await this.Create(new PeticionSoporte{
                FechaCreacion = DateTime.Now,
                UsuarioCreacion = UsuarioId,
                UsuarioPeticionario = UsuarioId,
                Abierta = true,
                Mensajes = new List<Message>{
                    new Message{
                        UserId = UsuarioId,
                        Msg = "Asunto: "+Asunto,
                        Fecha = DateTime.Now,
                        Leido = false
                    },
                    new Message{
                        UserId = UsuarioId,
                        Msg = "Descripción: "+Descripcion,
                        Fecha = DateTime.Now,
                        Leido = false
                    }
                },
            });
            if (creacion.Resultado != 0){
                return new RItem<PeticionSoporte>(null){
                    Mensaje = "No se ha podido crear la peticion",
                    Resultado = -1
                };
            }
            return creacion;
        }
            
    }
}