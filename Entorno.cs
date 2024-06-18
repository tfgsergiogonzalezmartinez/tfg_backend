using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_tfg
{
    public class Entorno
    {

        public bool PRODUCCION { get; set; }
        public string? IP { get; set; }
        public string? IP_PRODUCCION { get; set; }
        public string? IP_DESARROLLO { get; set; }
        public string ?DB_HOST { get; set; }
        public int? DB_PORT { get; set; }
        public string? DB_USER { get; set; }
        public string? DB_PASSWORD { get; set; }
        public string? DB_NAME { get; set; }

        public int? PORT { get; set; }
        public string? HOST { get; set; }

        public string? JWT_SECRET { get; set; }
        public string? JWT_AUDIENCE { get; set; }
        public string? JWT_ISSUER { get; set; }
        public string? JWT_EXPIRATION { get; set; }

        public string? EMAIL_HOST { get; set; }
        public int? EMAIL_PORT { get; set; }
        public string? EMAIL_USER { get; set; }
        public string? EMAIL_PASSWORD { get; set; }

        public string? LOG_LEVEL { get; set; }
        public string? APP_NAME { get; set; }
        public string? APP_VERSION { get; set; }
        public bool? FEATURE_FLAG_XYZ { get; set; }
        public string? POD_NAME { get; set; }
        public string? NAMESPACE { get; set; }
        
        
        public Entorno(IConfiguration configuration){
            PRODUCCION = Convert.ToBoolean(configuration["PRODUCCION"]);
            IP_PRODUCCION = configuration["IP_PRODUCCION"];
            IP_DESARROLLO = configuration["IP_DESARROLLO"];
            DB_HOST = configuration["DB_HOST"];
            DB_PORT = Convert.ToInt32(configuration["DB_PORT"]);
            DB_USER = configuration["DB_USER"];
            DB_PASSWORD = configuration["DB_PASSWORD"];
            DB_NAME = configuration["DB_NAME"];

            PORT = Convert.ToInt32(configuration["PORT"]);
            HOST = configuration["HOST"];
            
            JWT_SECRET = configuration["JWT_SECRET"];
            JWT_AUDIENCE = configuration["JWT_AUDIENCE"];
            JWT_ISSUER = configuration["JWT_ISSUER"];
            JWT_EXPIRATION= configuration["JWT_EXPIRATION"];

            EMAIL_HOST = configuration["EMAIL_HOST"];
            EMAIL_PORT = Convert.ToInt32(configuration["EMAIL_PORT"]);
            EMAIL_USER = configuration["EMAIL_USER"];
            EMAIL_PASSWORD = configuration["EMAIL_PASSWORD"];
            
            LOG_LEVEL = configuration["LOG_LEVEL"];
            APP_NAME = configuration["APP_NAME"];
            APP_VERSION = configuration["APP_VERSION"];
            FEATURE_FLAG_XYZ = Convert.ToBoolean(configuration["FEATURE_FLAG_XYZ"]);
            POD_NAME = configuration["POD_NAME"];
            NAMESPACE = configuration["NAMESPACE"];
        }
    
    
    }
}