using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto;
using backend_tfg.interfaces;
using backend_tfg.models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace backend_tfg.repositorios
{
public class BaseRepositorio<T> : IBaseRepositorio<T> where T : Entidad
    {
        protected readonly ContextoDB contexto;
        protected readonly IMongoCollection<T> collection;

        public BaseRepositorio(ContextoDB contexto)
        {
            this.contexto = contexto;
            this.collection = contexto.GetCollection<T>();
        }


        public async Task<RItem<T>> Create(T valor)
        {
            try
            {
                valor.FechaCreacion = DateTime.Now;
                await collection.InsertOneAsync(valor);

                return await GetById(valor.Id);
            }
            catch (Exception ex)
            {
                return new RItem<T>(null)
                {
                    Resultado = -1,
                    Mensaje = ex.Message + "\n" + ex.StackTrace
                };
            }
        }

        public async Task<RItem<T>> Put(T valor)
        {
            try
            {
                valor.FechaModificacion = DateTime.Now;
                var filter = Builders<T>.Filter.Eq(x => x.Id, valor.Id);
                await collection.ReplaceOneAsync(filter, valor);

                return await GetById(valor.Id);
            }
            catch (Exception ex)
            {
                return new RItem<T>(null)
                {
                    Resultado = -1,
                    Mensaje = ex.Message + "\n" + ex.StackTrace
                };
            }
        }

        public async Task<RLista<T>> GetAll()
        {
            try
            {
                var items = await collection.FindAsync(new BsonDocument()).Result.ToListAsync();
                return new RLista<T>(items);
            }
            catch (Exception ex)
            {
                return new RLista<T>(null)
                {
                    Resultado = -1,
                    Mensaje = ex.Message + "\n" + ex.StackTrace
                };
            }
        }

        public async Task<RItem<T>> GetById(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(x => x.Id, id);
                var item = await collection.FindAsync(filter).Result.FirstAsync();
                return new RItem<T>(item);
            }
            catch (Exception ex)
            {
                return new RItem<T>(null)
                {
                    Resultado = -1,
                    Mensaje = ex.Message + "\n" + ex.StackTrace
                };
            }
        }

        public async Task<Respuesta> Delete(string id)
        {
            try
            {
                var filter = Builders<T>.Filter.Eq(x => x.Id, id);
                var result = await collection.DeleteOneAsync(filter);

                if (result.DeletedCount > 0)
                {
                    return new Respuesta();
                }
                else
                {
                    return new Respuesta()
                    {
                        Resultado = -1,
                        Mensaje = "Elemento no encontrado"
                    };
                }
            }
            catch (Exception ex)
            {
                return new Respuesta()
                {
                    Resultado = -100,
                    Mensaje = ex.Message + "\n" + ex.StackTrace
                };
            }
        }
    }
}