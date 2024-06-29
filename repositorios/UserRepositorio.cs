using BCrypt.Net;
using System.Threading.Tasks;
using MongoDB.Driver;
using backend_tfg.interfaces;
using backend_tfg.modelos.usuario;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;
using backend_tfg.dto.UserDto;
using tfg_backend.dto.UserDto;
using MongoDB.Bson;

namespace backend_tfg.repositorios
{
    public class UserRepositorio : BaseRepositorio<User>, IUserRepositorio
    {
        private readonly IMongoCollection<User> _usuariosCollection;
        private IConfiguration _config;

        public UserRepositorio(IConfiguration config, ContextoDB contexto) : base(contexto)
        {
            this._usuariosCollection = contexto.GetCollection<User>();
            this._config = config;
        }


        public async Task<RItem<UserLoginGetDto>> Login(UserLoginDto usuarioLoginDTO)
        {
            if (!IsValidEmail(usuarioLoginDTO.Email))
            {
                return new RItem<UserLoginGetDto>(null)
                {
                    Resultado = -1,
                    Mensaje = "Email no válido"
                };
            }
            if (!IsValidPassword(usuarioLoginDTO.Password))
            {
                return new RItem<UserLoginGetDto>(null)
                {
                    Resultado = -2,
                    Mensaje = "Contraseña no valida, debe contener al menos una letra mayúscula y un número o un símbolo."
                };
            }

            var usuario = await _usuariosCollection.Find<User>(u => u.Email == usuarioLoginDTO.Email).FirstOrDefaultAsync();

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(usuarioLoginDTO.Password, usuario.HashedPassword))
            {
                return new RItem<UserLoginGetDto>(null)
                {
                    Mensaje = "Contraseña o Email incorrecto",
                    Resultado = -1
                };
            }
            usuario.FechaUltimoAcceso = System.DateTime.Now;
            await _usuariosCollection.ReplaceOneAsync(u => u.Id == usuario.Id, usuario);
            string token = GenerateJSONWebToken(usuario);
            UserLoginGetDto userLoginGetDto = new UserLoginGetDto(usuario);
            userLoginGetDto.Token = token;

            return new RItem<UserLoginGetDto>(userLoginGetDto)
            {
                Mensaje = "Inicio de sesión exitoso",
                Resultado = 0
            };
        }

        public async Task<RItem<UserLoginGetDto>> Register(UserCreateDto usuarioCreaDTO)
        {
            if (!IsValidEmail(usuarioCreaDTO.Email))
            {
                return new RItem<UserLoginGetDto>(null)
                {
                    Resultado = -1,
                    Mensaje = "Email no válido"
                };
            }
            if (!IsValidPassword(usuarioCreaDTO.Password))
            {
                return new RItem<UserLoginGetDto>(null)
                {
                    Resultado = -2,
                    Mensaje = "Contraseña no valida, debe contener al menos una letra mayúscula y un número o un símbolo."
                };
            }

            User usuario1 = await _usuariosCollection.Find<User>(u => u.Email == usuarioCreaDTO.Email).FirstOrDefaultAsync();
            if (usuario1 != null)
            {
                return new RItem<UserLoginGetDto>(null)
                {
                    Resultado = -1,
                    Mensaje = "Usuario ya existe"
                };
            }
            // encripto la contraseña
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuarioCreaDTO.Password);

            usuarioCreaDTO.Password = hashedPassword;
            User usuario = new User();
            usuarioCreaDTO.toEntidad(usuario);
            usuario.FechaCreacion = System.DateTime.Now;
            usuario.Listable = true;
            usuario.Rol = "user";


            await _usuariosCollection.InsertOneAsync(usuario);


            var user = await _usuariosCollection.Find<User>(u => u.Email == usuario.Email).FirstOrDefaultAsync();
            if (user == null)
            {
                return new RItem<UserLoginGetDto>(null)
                {
                    Resultado = -1,
                    Mensaje = "Error al crear usuario"
                };
            }
            //Creo la carpeta del usuario, en /data, aqui estara su foto de perfil y sus archivos.
            var rutaUsuario = Path.Combine("data", usuario.Id.ToString());
            if (!Directory.Exists(rutaUsuario))
            {
                Directory.CreateDirectory(rutaUsuario);
            }
            string avatarDefault = @"data/DefaultData/avatar.jpg";
            System.IO.File.Copy(avatarDefault, rutaUsuario + "/avatar.jpg");


            //genero el token y lo devuelvo para que inicio sesion automaticamente.
            var token = this.GenerateJSONWebToken(user);
            UserLoginGetDto userLoginGetDto = new UserLoginGetDto(user);
            userLoginGetDto.Token = token;
            return new RItem<UserLoginGetDto>(userLoginGetDto)
            {
                Resultado = 0,
                Mensaje = "Usuario creado exitosamente"
            };
        }

        public async Task<RItem<User>> CambiarPassword(UserCambiarPasswordDto userCambiarPassword)
        {
            var usuario = await _usuariosCollection.Find<User>(u => u.Email == userCambiarPassword.Email).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return new RItem<User>(null)
                {
                    Resultado = -1,
                    Mensaje = "Usuario no encontrado"
                };
            }
            if (!BCrypt.Net.BCrypt.Verify(userCambiarPassword.PasswordAntigua, usuario.HashedPassword))
            {
                return new RItem<User>(null)
                {
                    Resultado = -1,
                    Mensaje = "Contraseña incorrecta"
                };
            }

            if (userCambiarPassword.PasswordNueva1 != userCambiarPassword.PasswordNueva2)
            {
                return new RItem<User>(null)
                {
                    Resultado = -1,
                    Mensaje = "Las contraseñas no coinciden"
                };
            }


         


            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userCambiarPassword.PasswordNueva1);
            usuario.HashedPassword = hashedPassword;

            var resultado = await _usuariosCollection.ReplaceOneAsync(u => u.Id == usuario.Id, usuario);

            if (resultado.ModifiedCount == 0)
            {
                return new RItem<User>(null)
                {
                    Resultado = -1,
                    Mensaje = "Error al cambiar la contraseña"
                };
            }


            return new RItem<User>(usuario)
            {
                Resultado = 0,
                Mensaje = "Contraseña cambiada correctamente"
            };

        }
        public async Task<RItem<User>> ModificarRol(UserCambiarRolDto userModificarRolDto){
            var usuario = await _usuariosCollection.Find<User>(u => u.Email == userModificarRolDto.Email).FirstOrDefaultAsync();
            if (usuario == null)
            {
                return new RItem<User>(null)
                {
                    Resultado = -1,
                    Mensaje = "Usuario no encontrado"
                };
            }
            usuario.Rol = userModificarRolDto.Rol;
            var resultado = await _usuariosCollection.ReplaceOneAsync(u => u.Id == usuario.Id, usuario);
            if (resultado.ModifiedCount == 0)
            {
                return new RItem<User>(null)
                {
                    Resultado = -1,
                    Mensaje = "Error al cambiar el rol"
                };
            }
            return new RItem<User>(usuario);
    
        }





        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_SECRET"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Creo una lista de claims con la información del usuario
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Nombre),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
                new Claim("DateOfJoining", userInfo.FechaCreacion?.ToString("yyyy-MM-dd")),
                new Claim(ClaimTypes.Role, userInfo.Rol)
            };

            var issuer = _config["JWT_ISSUER"];
            var audience = _config["JWT_AUDIENCE"];
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["JWT_EXPIRATION"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
        private bool IsValidPassword(string password)
        {
            if (password.Length < 6)
            {
                return false;
            }

            bool hasUpperCase = false;
            bool hasNumberOrSymbol = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                {
                    hasUpperCase = true;
                }
                else if (char.IsDigit(c) || char.IsSymbol(c) || char.IsPunctuation(c))
                {
                    hasNumberOrSymbol = true;
                }
            }

            return hasUpperCase && hasNumberOrSymbol;
        }

        public async Task<RLista<User>> ObtenerUsuariosCoincidentes(string nombre){
            var filter = Builders<User>.Filter.Regex("Nombre", new BsonRegularExpression(nombre, "i"));
            var usuarios = await collection.Find(filter).Limit(5).ToListAsync();
            return new RLista<User>(usuarios);
        }

        


    }
}

