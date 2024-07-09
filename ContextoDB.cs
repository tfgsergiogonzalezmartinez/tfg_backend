using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.modelos.usuario;
using BCrypt.Net;
using MongoDB.Driver;
using Newtonsoft.Json;
using tfg_backend.modelos.Plantilla;
using tfg_backend.modelos.Proyecto;

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

        public IMongoDatabase getDatabase()
        {
            return _database;
        }
        public IMongoCollection<T> GetCollection<T>()
        {
            string nombre = string.Empty;
            if (typeof(T).Name.Equals("User"))
            {
                nombre = "User";
            }
            else if (typeof(T).Name.Equals("Chat"))
            {
                nombre = "Chat";
            }
            else if (typeof(T).Name.Equals("PeticionSoporte"))
            {
                nombre = "Soporte";
            }
            else if (typeof(T).Name.Equals("Plantilla"))
            {
                nombre = "Plantilla";
            }
            else if (typeof(T).Name.Equals("Proyecto"))
            {
                nombre = "Proyecto";
            }

            return _database.GetCollection<T>(nombre);
        }


        protected async void InicializarDatos()
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
                        HashedPassword = BCrypt.Net.BCrypt.HashPassword("Admin7#"),
                        FechaNacimiento = DateTime.Now,
                        Rol = "admin",
                        Listable = true,
                        FechaCreacion = DateTime.Now,
                    },
                });
                var user = await usuarios.Find(u => u.Email == "admin@admin.es").FirstOrDefaultAsync();
                var rutaUsuario = Path.Combine("data", user.Id.ToString());
                if (!Directory.Exists(rutaUsuario))
                {
                    Directory.CreateDirectory(rutaUsuario);
                }
                string avatarDefault = @"data/DefaultData/avatar.jpg";
                System.IO.File.Copy(avatarDefault, rutaUsuario + "/avatar.jpg");
            }


            var plantillas = _database.GetCollection<Plantilla>("Plantilla");

            if (plantillas.CountDocuments(FilterDefinition<Plantilla>.Empty) == 0)
            {
                plantillas.InsertOne(new Plantilla
                {
                    Nombre = "Tienda",
                    Modelos = new List<ModeloPlantilla>
                    {
                        new ModeloPlantilla
                        {
                            Nombre = "Producto",
                            Campos = new List<string>
                            {
                                "Nombre",
                                "Descripcion",
                                "Precio",
                                "Stock",
                                "Colores",
                                "Tallas",
                                "FotoPrincipal",
                                "Fotos",
                                "Categoria"
                            }
                        },
                        new ModeloPlantilla
                        {
                            Nombre = "Categoria",
                            Campos = new List<string>
                            {
                                "Nombre",
                                "CategoriaPadre"
                            }
                        }
                    }
                });
            }

        }
    }
}