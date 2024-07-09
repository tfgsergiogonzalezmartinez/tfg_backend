using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tfg_backend.modelos.Proyecto
{
    public class Personalizacion
    {
        public string? Logo { get; set; }
        public string? Extension { get; set; }
        public string? Moneda { get; set; }

        public string? Titulo { get; set; }

        public string Color_backgound { get; set; }
        public string Color_backgound_light { get; set; }
        public string Color_backgound_dark { get; set; }

        public string Color_items { get; set; }
        public string Color_items_light { get; set; }
        public string Color_items_dark { get; set; }

        public string Color_texto { get; set; }
        public string Color_texto_light { get; set; }
        public string Color_texto_dark { get; set; }

        public string Color_boton { get; set; }
        public string Color_boton_light { get; set; }
        public string Color_boton_dark { get; set; }

        public string Color_header { get; set; }
        public string Color_header_light { get; set; }
        public string Color_header_dark { get; set; }

        public string Color_subHeader { get; set; }
        public string Color_subHeader_light { get; set; }
        public string Color_subHeader_dark { get; set; }
    }
}