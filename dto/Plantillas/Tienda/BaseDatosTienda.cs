using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tfg_backend.dto.Plantillas.Tienda
{
    public class BaseDatosTienda
    {
        public List<Producto> Productos {get; set;}
        public List<Categoria> Categorias {get; set;}
    }
}