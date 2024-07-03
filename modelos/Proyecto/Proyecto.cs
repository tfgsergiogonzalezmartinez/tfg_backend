using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.models;

namespace tfg_backend.modelos.Proyecto
{
    public class Proyecto : Entidad
    {
        public string Usuario {get; set;}
        public string Nombre {get; set;}
        public string Plantilla {get; set;}
        public string Ruta {get; set;}
        public Personalizacion Personalizacion {get; set;}
    }
}