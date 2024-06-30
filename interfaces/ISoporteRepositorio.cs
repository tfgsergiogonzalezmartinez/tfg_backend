using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg.interfaces;
using backend_tfg.repositorios;
using tfg_backend.dto.SoporteDto;

namespace tfg_backend.interfaces
{
    public interface ISoporteRepositorio : IBaseRepositorio<PeticionSoporte>
    {
        Task<RLista<PeticionSoporte>> GetPeticionByUsuario(string usuarioId);
        Task<RItem<PeticionSoporte>> AbrirPeticion(string peticionId);
        Task<RItem<PeticionSoporte>> CerrarPeticion(string peticionId, string idUserAdmin);
        Task<RLista<PeticionSoporte>> GetPeticionesCerradas();
        Task<RLista<PeticionSoporte>> GetPeticionesAbiertas();
        Task<RItem<PeticionSoporte>> AsignarPeticionAdmin(string idPeticion, string idAdmin);
        Task<RItem<PeticionSoporte>> PostMessageUsers(string idPeticion ,NewMessage newMsg);
        Task<RItem<PeticionSoporte>> NuevaPeticion(string Asunto ,string Descripcion, string UsuarioId);
        
        
    }
}