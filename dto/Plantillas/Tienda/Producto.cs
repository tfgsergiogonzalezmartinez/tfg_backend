using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tfg_backend.dto.Plantillas.Tienda
{
    public class Producto
    {
        public string Nombre {get; set;}
        public string Descripcion {get; set;}
        public double Precio {get; set;}
        public int Stock {get; set;}
        public List<string> Fotos {get; set;}
        public string FotoPrincipal {get; set;}
        public string Categoria {get; set;}
        public List<string> Tallas {get; set;}
        public List<string> Colores {get; set;}   
    }
}