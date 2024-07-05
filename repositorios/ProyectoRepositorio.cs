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
        private string extensionLogo; 
        public ProyectoRepositorio(ContextoDB contexto, IConfiguration config) : base(contexto)
        {
            _config = config;
        }

        public async Task<RLista<Proyecto>> getProyectosUsuario(string idUsuario)
        {
            var datos = await collection.Find(p => p.Usuario == idUsuario).ToListAsync();

            if (datos is null)
            {
                return new RLista<Proyecto>(null)
                {
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
            if (dato.Resultado != 0)
            {
                return new RItem<Proyecto>(null)
                {
                    Mensaje = "Error al crear el proyecto",
                    Resultado = -1
                };
            }



            this.InicializarPlantilla(crearProyectoDto);
            this.ImplementarPersonalizacionAsync(crearProyectoDto);
            this.GenerarBD(crearProyectoDto);
            this.GenerarProyectoPlantilla(crearProyectoDto);
            this.LimpiezaProyecto(crearProyectoDto);



            return dato;
        }

        private void InicializarPlantilla(CrearProyectoDto crearProyectoDto)
        {
            string rutaPlantillaCopiar = Path.Combine("plantillas", crearProyectoDto.Plantilla, crearProyectoDto.Plantilla + ".zip");
            string rutaPegar = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla + ".zip");
            string rutaProyectoUsuario = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla);
            Directory.CreateDirectory(Path.GetDirectoryName(rutaProyectoUsuario));

            File.Copy(rutaPlantillaCopiar, rutaPegar, overwrite: true);
            ZipFile.ExtractToDirectory(rutaPegar, rutaProyectoUsuario);
        }

        private void GenerarProyectoPlantilla(CrearProyectoDto crearProyectoDto)
        {
            string rutaDestino = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla);
            string rutaCarpetaComprimida = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Usuario + crearProyectoDto.Nombre + "v1" + ".zip");
            ZipFile.CreateFromDirectory(rutaDestino, rutaCarpetaComprimida);

        }
        private void LimpiezaProyecto(CrearProyectoDto crearProyectoDto)
        {
            string rutaZipResidual = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla + ".zip");
            string rutaDirectorioResidual = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla);
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

        private async Task ImplementarPersonalizacionAsync(CrearProyectoDto crearProyectoDto)
        {
            string rutaEnv = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla, ".env");
            using (StreamWriter sw = new StreamWriter(rutaEnv, true))
            {
                sw.WriteLine("Titulo=" + crearProyectoDto.Personalizacion.Titulo);
                sw.WriteLine("Moneda=" + crearProyectoDto.Personalizacion.Moneda);
                sw.WriteLine("Logo_Extension=" + this.extensionLogo);
                sw.WriteLine("Color_background=" + "'"+crearProyectoDto.Personalizacion.Color_backgound+ "'");
                sw.WriteLine("Color_background_light=" + "'"+ crearProyectoDto.Personalizacion.Color_backgound_light+ "'");
                sw.WriteLine("Color_background_dark=" + "'"+ crearProyectoDto.Personalizacion.Color_backgound_dark+ "'");
                sw.WriteLine("Color_items=" +  "'"+crearProyectoDto.Personalizacion.Color_items+ "'");
                sw.WriteLine("Color_items_light=" + "'"+ crearProyectoDto.Personalizacion.Color_items_light+ "'");
                sw.WriteLine("Color_items_dark=" + "'"+ crearProyectoDto.Personalizacion.Color_items_dark+ "'");
                sw.WriteLine("Color_texto=" + "'"+ crearProyectoDto.Personalizacion.Color_texto+ "'");
                sw.WriteLine("Color_texto_light=" + "'"+ crearProyectoDto.Personalizacion.Color_texto_light+ "'");
                sw.WriteLine("Color_texto_dark=" + "'"+ crearProyectoDto.Personalizacion.Color_texto_dark+ "'");
                sw.WriteLine("Color_boton=" + "'"+ crearProyectoDto.Personalizacion.Color_boton+ "'");
                sw.WriteLine("Color_boton_light=" + "'"+ crearProyectoDto.Personalizacion.Color_boton_light+ "'");
                sw.WriteLine("Color_boton_dark=" + "'"+ crearProyectoDto.Personalizacion.Color_boton_dark+ "'");
                sw.WriteLine("Color_header=" + "'"+crearProyectoDto.Personalizacion.Color_header+ "'");
                sw.WriteLine("Color_header_light=" + "'"+ crearProyectoDto.Personalizacion.Color_header_light+ "'");
                sw.WriteLine("Color_header_dark=" + "'"+ crearProyectoDto.Personalizacion.Color_header_dark+ "'");
                sw.WriteLine("Color_subHeader=" + "'"+ crearProyectoDto.Personalizacion.Color_subHeader+ "'");
                sw.WriteLine("Color_subHeader_light=" + "'"+ crearProyectoDto.Personalizacion.Color_subHeader_light+ "'");
                sw.WriteLine("Color_subHeader_dark=" + "'"+ crearProyectoDto.Personalizacion.Color_subHeader_dark+ "'");
            }
   
            this.cargarFoto(crearProyectoDto);



        }

        private void GenerarBD(CrearProyectoDto crearProyectoDto)
        {
            var ProductosJson = JsonSerializer.Serialize(crearProyectoDto.BaseDatosTienda.Productos);
            var CategoriaJson = JsonSerializer.Serialize(crearProyectoDto.BaseDatosTienda.Categorias);

            string rutaArchivo = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla, "plantila_tienda_backend", "dbModelos", "bbdd_productos.json");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
            File.WriteAllText(rutaArchivo, ProductosJson);

            rutaArchivo = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla, "plantila_tienda_backend", "dbModelos", "bbdd_categorias.json");
            Directory.CreateDirectory(Path.GetDirectoryName(rutaArchivo));
            File.WriteAllText(rutaArchivo, CategoriaJson);
        }

        private async void cargarFoto(CrearProyectoDto crearProyectoDto){
            var dataUriHeader = crearProyectoDto.Personalizacion.Logo.Split(',')[0];
            var base64Data = crearProyectoDto.Personalizacion.Logo.Split(',')[1];
            this.extensionLogo = ".jpg";

            if (dataUriHeader.Contains("image/jpeg"))
            {
                this.extensionLogo = ".jpg";
            }
            else if (dataUriHeader.Contains("image/png"))
            {
                this.extensionLogo = ".png";
            }
            else if (dataUriHeader.Contains("image/gif"))
            {
                this.extensionLogo = ".gif";
            }
            else if (dataUriHeader.Contains("image/webp"))
            {
                this.extensionLogo = ".webp";
            }
 
            var rutaCarpeta = Path.Combine("data", crearProyectoDto.Usuario, crearProyectoDto.Nombre, crearProyectoDto.Plantilla, "plantila_tienda_frontend","src","assets","logos");
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            var validExtensions = new[] { ".jpg", ".png", ".gif", ".webp" };
            foreach (var ext in validExtensions)
            {
                var existingAvatarPath = Path.Combine(rutaCarpeta, "logo" + ext);
                if (System.IO.File.Exists(existingAvatarPath))
                {
                    System.IO.File.Delete(existingAvatarPath);
                }
            }

            var rutaArchivo = Path.Combine(rutaCarpeta, "logo" + this.extensionLogo);
            var imageData = Convert.FromBase64String(base64Data);
            await System.IO.File.WriteAllBytesAsync(rutaArchivo, imageData);
        }


    }
}