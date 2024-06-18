using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend_tfg.dto
{
    public class Filtro
    {
        public string Campo{get;set;}=string.Empty;
        public string Tipo{get;set;}="=";
        public object Valor{get;set;}=new();
        public bool Negar { get; set; } = false;
    }
}