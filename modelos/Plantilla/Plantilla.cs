using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.models;

namespace tfg_backend.modelos.Plantilla
{
    public class Plantilla : Entidad
    {
        public string Nombre {get; set;}
        public List<ModeloPlantilla> Modelos {get; set;}
        
    }
}