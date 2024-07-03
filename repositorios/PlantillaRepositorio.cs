using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_tfg;
using backend_tfg.interfaces;
using backend_tfg.repositorios;
using tfg_backend.interfaces;
using tfg_backend.modelos.Plantilla;

namespace tfg_backend.repositorios
{
    public class PlantillaRepositorio : BaseRepositorio<Plantilla>, IPlantillaRepositorio
    {
        private IConfiguration _config;
        public PlantillaRepositorio(ContextoDB contextoDb, IConfiguration config) : base(contextoDb)
        {
            _config = config;
        }        
    }
}