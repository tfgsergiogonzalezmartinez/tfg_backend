using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace backend_tfg.models
{
    public class Entidad
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; } 
        public DateTime? FechaUltimoAcceso { get; set; } 
        public string UsuarioCreacion { get; set; } = string.Empty;
        public string UsuarioModificacion { get; set; } = string.Empty;
    }
}