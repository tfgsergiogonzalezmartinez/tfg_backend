using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.models;

namespace backend_tfg.dto
{
    public class EntidadGetDto 
    {
        public string Id { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime? FechaUltimoAcceso { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        

        public EntidadGetDto()
        {
          
        }
        public EntidadGetDto(Entidad entidad){
            Id = entidad.Id;
            FechaCreacion = entidad.FechaCreacion;
            FechaModificacion = entidad.FechaModificacion;
            FechaUltimoAcceso = entidad.FechaUltimoAcceso;
            UsuarioCreacion = entidad.UsuarioCreacion;
            UsuarioModificacion = entidad.UsuarioModificacion;
        }
        

       
        public Entidad toEntidad(Entidad entidad ){
            entidad.Id = this.Id;
            entidad.FechaCreacion = this.FechaCreacion;
            entidad.FechaModificacion = this.FechaModificacion;
            entidad.FechaUltimoAcceso = this.FechaUltimoAcceso;
            entidad.UsuarioCreacion = this.UsuarioCreacion;
            entidad.UsuarioModificacion = this.UsuarioModificacion;
            return entidad;
        }
    }
}