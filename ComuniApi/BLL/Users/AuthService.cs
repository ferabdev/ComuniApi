using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace ComuniApi.BLL.Users
{
    public class AuthService
    {
        private readonly ComuniContext _context;
        private readonly PasswordHasher<UsuarioEntity> _hasher = new();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(ComuniContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GenericResponse<UserModel>> RegisterAsync(UserModelReq model)
        {
            model.CodigoComunidad = model.CodigoComunidad.ToUpper();

            var comunidad = await _context.Comunidades.FirstOrDefaultAsync(c => c.CodigoComunidad == model.CodigoComunidad);

            if (comunidad == null) return new GenericResponse<UserModel>
            {
                Status = HttpStatusCode.Conflict,
                Message = "La comunidad no se encontró",
            };

            // Verificar si ya existe
            var exists = await _context.Usuarios
                .AnyAsync(u => u.Id == comunidad.Id 
                && (u.Username == model.Usuario || u.Email == model.Correo));

            if (exists) return new GenericResponse<UserModel>
            {
                Status = HttpStatusCode.Conflict,
                Message = "El nombre de usuario o correo ya está en uso",
            };

            var usuario = new UsuarioEntity
            {
                Username = model.Usuario,
                NombreCompleto = model.Nombre,
                Email = model.Correo,
                ComunidadId = comunidad.Id,
                RolId = 1
            };

            usuario.PasswordHash = _hasher.HashPassword(usuario, model.Password);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return new GenericResponse<UserModel>
            {
                Status = HttpStatusCode.Created,
                Message = "Usuario registrado exitosamente",
                Data = new UserModel
                {
                    Id = usuario.Id,
                    Usuario = usuario.Username,
                    Nombre = usuario.NombreCompleto,
                    Correo = usuario.Email,
                    CodigoComunidad = comunidad.CodigoComunidad,
                    Comunidad = comunidad.Nombre,
                    Rol = "Usuario"
                },
            };
        }

        public async Task<GenericResponse<UserModel>> Login(AuthReq model)   
        {
            try
            {
                model.CodigoComunidad = model.CodigoComunidad.ToUpper();
                var usuario = await _context.Usuarios
                    .Include(u => u.Comunidad)
                    .Include(u => u.Rol)
                    .SingleOrDefaultAsync(u => u.Comunidad.CodigoComunidad == model.CodigoComunidad && u.Username == model.Username);
                if (usuario == null) return new GenericResponse<UserModel>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Message = "Usuario no encontrado",
                };

                var result = _hasher.VerifyHashedPassword(usuario, usuario.PasswordHash, model.Password);
                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    // Si result == SuccessRehashNeeded, considera rehash al hacer login (opcional)
                    if (result == PasswordVerificationResult.SuccessRehashNeeded)
                    {
                        usuario.PasswordHash = _hasher.HashPassword(usuario, model.Password);
                        _context.Usuarios.Update(usuario);
                        await _context.SaveChangesAsync();
                    }

                    return new GenericResponse<UserModel>
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Login exitoso",
                        Data = new UserModel
                        {
                            Id = usuario.Id,
                            Usuario = usuario.Username,
                            Nombre = usuario.NombreCompleto,
                            Correo = usuario.Email,
                            CodigoComunidad = usuario.Comunidad.CodigoComunidad,
                            Comunidad = usuario.Comunidad.Nombre,
                            Rol = usuario.Rol.Descripcion
                        },
                    };
                }

                return new GenericResponse<UserModel>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Message = "Credenciales inválidas",
                };
            }
            catch (Exception ex) 
            {
                return new GenericResponse<UserModel>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al procesar el login",
                    ExtraInfo = ex.Message
                };
            }
        }

        public int? ObtenerIdUsuario()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) throw new Exception("No se encontro el httpcontext");

            var user = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            int? userId = user != null ? int.Parse(user) : null;

            return userId;
        }

        public string ObtenerCodigoComunidad()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null) throw new Exception("No se encontro el httpcontext");

            var CodigoComunidadClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == "CodigoComunidad");
            var CodigoComunidad = CodigoComunidadClaim?.Value ?? "";

            return CodigoComunidad;
        }
    }
}
