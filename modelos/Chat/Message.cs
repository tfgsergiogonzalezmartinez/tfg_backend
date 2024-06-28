using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend_tfg.modelos.EntidadChat
{
    public class Message
    {
        public string UserId { get; set; } = string.Empty;
        public string Msg { get; set; } = string.Empty;
        public bool Leido { get; set; } = false;
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime? Fecha { get; set; } = new DateTime(0);
    }
}