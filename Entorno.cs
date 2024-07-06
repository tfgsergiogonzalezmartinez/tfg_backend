using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_tfg
{
    public class Entorno
    {

        public string? IP_FRONT { get; set; }

        
        public string ?DB_HOST { get; set; }
        public int? DB_PORT { get; set; }
        public string? DB_USER { get; set; }
        public string? DB_PASSWORD { get; set; }
        public string? DB_NAME { get; set; }

        public string? JWT_SECRET { get; set; }
        public string? JWT_AUDIENCE { get; set; }
        public string? JWT_ISSUER { get; set; }
        public string? JWT_EXPIRATION { get; set; }

        public string? APP_NAME { get; set; }
        public string? APP_VERSION { get; set; }
        
        
        public Entorno(IConfiguration configuration){
            IP_FRONT = configuration["IP_FRONT"];

            DB_HOST = configuration["DB_HOST"];
            DB_PORT = Convert.ToInt32(configuration["DB_PORT"]);
            DB_USER = configuration["DB_USER"];
            DB_PASSWORD = configuration["DB_PASSWORD"];
            DB_NAME = configuration["DB_NAME"];

            
            JWT_SECRET = configuration["JWT_SECRET"];
            JWT_AUDIENCE = configuration["JWT_AUDIENCE"];
            JWT_ISSUER = configuration["JWT_ISSUER"];
            JWT_EXPIRATION= configuration["JWT_EXPIRATION"];

            
            APP_NAME = configuration["APP_NAME"];
            APP_VERSION = configuration["APP_VERSION"];
        }
    
    
    }
}