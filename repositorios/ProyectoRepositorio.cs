using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using backend_tfg;
using backend_tfg.interfaces;
using backend_tfg.repositorios;
using MongoDB.Driver;
using tfg_backend.dto.Proyecto;
using tfg_backend.interfaces;
using tfg_backend.modelos.Proyecto;

namespace tfg_backend.repositorios
{
    public class ProyectoRepositorio : BaseRepositorio<Proyecto>, IProyectoRepositorio
    {
        private IConfiguration _config;
        public ProyectoRepositorio(ContextoDB contexto, IConfiguration config) : base(contexto)
        {
            _config = config;
        }

        public async Task<RLista<Proyecto>> getProyectosUsuario(string idUsuario)
        {
            var datos = await collection.Find(p => p.Usuario == idUsuario).ToListAsync();

            if (datos is null){
                return new RLista<Proyecto>(null){
                    Mensaje = "No se ha encontrado ningun proyecto con ese usuario",
                    Resultado = 0
                };
            }
            return new RLista<Proyecto>(datos);
        }

        public async Task<RItem<Proyecto>> GenerarProyecto(CrearProyectoDto crearProyectoDto)
        {
            Proyecto proyecto = new Proyecto();
            proyecto.Nombre = crearProyectoDto.Nombre;
            proyecto.Usuario = crearProyectoDto.Usuario;
            proyecto.Plantilla = crearProyectoDto.Plantilla;
            proyecto.Ruta = crearProyectoDto.Ruta;
            proyecto.Personalizacion = crearProyectoDto.Personalizacion;
            var dato = await this.Create(proyecto);
            if (dato.Resultado != 0){
                return new RItem<Proyecto>(null){
                    Mensaje = "Error al crear el proyecto",
                    Resultado = -1
                };
            }

            var ProductosJson = JsonSerializer.Serialize(crearProyectoDto.BaseDatosTienda.Productos);
            var CategoriaJson = JsonSerializer.Serialize(crearProyectoDto.BaseDatosTienda.Categorias);

            this.InicializarPlantilla(crearProyectoDto);
            string rutaArchivo = Path.Combine("data", proyecto.Usuario, proyecto.Nombre, crearProyectoDto.Plantilla,"plantila_tienda_backend", "dbModelos", "bbdd_productos.json");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
            File.WriteAllText(rutaArchivo, ProductosJson);
          
            rutaArchivo = Path.Combine("data", proyecto.Usuario, proyecto.Nombre,crearProyectoDto.Plantilla,"plantila_tienda_backend","dbModelos", "bbdd_categorias.json");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
            File.WriteAllText(rutaArchivo, CategoriaJson);


            this.GenerarProyectoPlantilla(crearProyectoDto);
            this.LimpiezaProyecto(crearProyectoDto);



            return dato;
        }

        private void InicializarPlantilla(CrearProyectoDto crearProyectoDto ){
            string rutaPlantillaCopiar = Path.Combine("plantillas",crearProyectoDto.Plantilla,crearProyectoDto.Plantilla+".zip");
            string rutaPegar = Path.Combine("data",crearProyectoDto.Usuario,crearProyectoDto.Nombre, crearProyectoDto.Plantilla+ ".zip");
            string rutaProyectoUsuario = Path.Combine("data",crearProyectoDto.Usuario,crearProyectoDto.Nombre, crearProyectoDto.Plantilla);
            Directory.CreateDirectory(Path.GetDirectoryName(rutaProyectoUsuario));

            File.Copy(rutaPlantillaCopiar, rutaPegar, overwrite: true);
            ZipFile.ExtractToDirectory(rutaPegar, rutaProyectoUsuario);
        }

        private void GenerarProyectoPlantilla(CrearProyectoDto crearProyectoDto){
            string rutaDestino = Path.Combine("data",crearProyectoDto.Usuario,crearProyectoDto.Nombre, crearProyectoDto.Plantilla);
            string rutaCarpetaComprimida = Path.Combine("data",crearProyectoDto.Usuario,crearProyectoDto.Nombre,crearProyectoDto.Usuario +crearProyectoDto.Nombre+"v1" + ".zip");
            ZipFile.CreateFromDirectory(rutaDestino, rutaCarpetaComprimida);

        }
        private void LimpiezaProyecto(CrearProyectoDto crearProyectoDto){
        string rutaZipResidual = Path.Combine("data",crearProyectoDto.Usuario,crearProyectoDto.Nombre, crearProyectoDto.Plantilla+ ".zip");
        string rutaDirectorioResidual = Path.Combine("data",crearProyectoDto.Usuario,crearProyectoDto.Nombre, crearProyectoDto.Plantilla);
        if (File.Exists(rutaZipResidual))
        {
            File.Delete(rutaZipResidual);
        }
        if (Directory.Exists(rutaDirectorioResidual))
        {
            Directory.Delete(rutaDirectorioResidual, true);
            Console.WriteLine("Directorio de proyecto usuario eliminado exitosamente.");
        }
        }


    }
}