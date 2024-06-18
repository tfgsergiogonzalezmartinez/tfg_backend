using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.dto;

namespace backend_tfg.interfaces
{
    public record Respuesta(){
        public int Resultado { get; set; }=0;
        public string Mensaje {get;set;}="Ok";
    };
    public record RLista<T>(List<T>? Lista):Respuesta();
    public record RItem<T> (T? Valor):Respuesta();
    
    public interface IBaseRepositorio<T>
    where T: class
    {
       Task<RItem<T>> Create(T valor);
       Task<RItem<T>> Put(T valor);
       Task<RLista<T>> GetAll();
       Task<RItem<T>> GetById(string id);
       Task<Respuesta> Delete(string id);
    }
}