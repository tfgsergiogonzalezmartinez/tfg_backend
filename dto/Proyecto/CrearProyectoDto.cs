using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tfg_backend.dto.Plantillas.Tienda;
using tfg_backend.modelos.Proyecto;

namespace tfg_backend.dto.Proyecto
{
    public class CrearProyectoDto
    {
        public string Usuario {get; set;}
        public string Nombre {get; set;}
        public string Plantilla {get; set;}
        public string? Ruta {get; set;}
        public Personalizacion Personalizacion {get; set;}
        public BaseDatosTienda BaseDatosTienda {get; set;}
        
    }
}