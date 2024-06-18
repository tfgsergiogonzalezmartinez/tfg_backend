using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace backend_tfg
{
    public class MongoDbSettings
    {
        private readonly IMongoDatabase _database;


        public MongoDbSettings( string ConnectionString, string DatabaseName)
        {
            var client = new MongoClient(ConnectionString);
            _database = client.GetDatabase(DatabaseName);
        }
    }
}