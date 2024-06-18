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

        public async Task<RItem<User>> CambiarContraseña(User usuario)
        {
            var usuarioActual = await _usuariosCollection.Find<User>(u => u.Id == usuario.Id).FirstOrDefaultAsync();
            if (usuarioActual == null)
            {
                return new RItem<User>(null)
                {
                    Mensaje = "Usuario no encontrado",
                    Resultado = -1
                };
            }
            return new RItem<User>(null)
            {
                Mensaje = "Usuario no encontrado",
                Resultado = -1
            };
        }

        public async Task<RItem<UserLoginGetDto>> Login(UserLoginDTO usuarioLoginDTO)
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

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(usuarioLoginDTO.Password, usuario.Password))
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

                public async Task<RItem<User>> Register(UserCreateDto usuarioCreaDTO)
        {
            if (!IsValidEmail(usuarioCreaDTO.Email))
            {
                return new RItem<User>(null)
                { 
                    Resultado = -1,
                    Mensaje = "Email no válido"
                };
            }
            if (!IsValidPassword(usuarioCreaDTO.Password))
            {
                return new RItem<User>(null)
                {
                    Resultado = -2,
                    Mensaje = "Contraseña no valida, debe contener al menos una letra mayúscula y un número o un símbolo."
                };
            }

            User usuario1 = await _usuariosCollection.Find<User>(u => u.Email == usuarioCreaDTO.Email).FirstOrDefaultAsync();
            if (usuario1 != null)
            {
                return new RItem<User>(null)
                {
                    Resultado = -1,
                    Mensaje = "Usuario ya existe"
                };
            }
            // Encrypt password before saving
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(usuarioCreaDTO.Password);

            usuarioCreaDTO.Password = hashedPassword;
            User usuario = new User();
            usuarioCreaDTO.toEntidad(usuario);
            usuario.FechaCreacion = System.DateTime.Now;
            usuario.Listable = true;
            usuario.Rol = "user";
            await _usuariosCollection.InsertOneAsync(usuario);
            return new RItem<User>(usuario)
            {
                Resultado = 0,
                Mensaje = "Usuario creado"
            };
        }





        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT_SECRET"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Create the list of claims with user information
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

            // Return the token
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
    }
}

