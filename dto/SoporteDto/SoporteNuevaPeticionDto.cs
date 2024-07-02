using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tfg_backend.dto.SoporteDto
{
    public class SoporteNuevaPeticionDto
    {
        public string UserId { get; set; }
        public string Asunto { get; set; }
        public string Descripcion { get; set; }
    }
}