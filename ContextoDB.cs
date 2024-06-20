using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.usuario;
using BCrypt.Net;
using MongoDB.Driver;

namespace backend_tfg
{
public class ContextoDB
    {
        private readonly IMongoDatabase _database;


        public ContextoDB(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
            InicializarDatos();
        }

        public IMongoDatabase getDatabase(){
            return _database;
        }
        public IMongoCollection<T> GetCollection<T>()
        {
            string nombre=string.Empty; 
            if (typeof(T).Name.Equals("User")) {
                nombre = "User";
            } else if (typeof(T).Name.Equals("Chat")) {
                nombre = "Chat";
            }
     
            return _database.GetCollection<T>(nombre);
        }
  
  
        protected void InicializarDatos()
        {

            var usuarios = _database.GetCollection<User>("User");
            if (usuarios.CountDocuments(FilterDefinition<User>.Empty) == 0)
            {
                usuarios.InsertMany(new List<User>
                {
                    new User
                    {
                        Nombre = "Sergio",
                        Apellido1 = "Gonzalez",
                        Apellido2 = "Martinez",
                        Email = "admin@admin.es",
                        HashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin7"),
                        FechaNacimiento = DateTime.Now,
                        Rol = "admin",
                        Listable = true,
                        FechaCreacion = DateTime.Now,
                    },
                });
            }
            
        }
    }
}