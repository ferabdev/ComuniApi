using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComuniApi.BLL.Comunidades
{
    public class ComunidadService
    {
        private readonly ComuniContext _context;

        public ComunidadService(ComuniContext context)
        {
            _context = context;
        }

        public async Task<GenericResponse<List<UsuariosComunidadRes>>> ObtenerUsuariosComunidad(int comunidadId)
        {
            try
            {
                var usuarios = await _context.Usuarios
                    .Where(u => u.ComunidadId == comunidadId)
                    .Select(u => new UsuariosComunidadRes
                    {
                        Id = u.Id,
                        Username = u.Username,
                        NombreCompleto = u.NombreCompleto,
                        Saldo = u.EdoCuentas.Sum(e => e.Monto)
                    })
                    .ToListAsync();

                return new GenericResponse<List<UsuariosComunidadRes>>
                {
                    Status = System.Net.HttpStatusCode.OK,
                    Message = "Usuarios obtenidos exitosamente.",
                    Data = usuarios
                };
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes registrar el error o lanzar una excepción personalizada)
                throw new ApplicationException("Error al obtener los usuarios de la comunidad.", ex);
            }
        }

        public async Task<GenericResponse<string>> CrearComunidad(ComunidadReq model)
        {
            try
            {
                string code;

                do
                {
                    code = GenerateCommunityCode(6);
                }
                while (await _context.Comunidades.AnyAsync(c => c.CodigoComunidad == code));

                var nuevaComunidad = new ComunidadEntity
                {
                    Nombre = model.Nombre,
                    Direccion = model.Direccion,
                    Correo = model.Correo,
                    MaxUsers = model.MaxUsers,
                    Usuarios = new List<UsuarioEntity>(),
                    CodigoComunidad = code
                };

                PasswordHasher<UsuarioEntity> _hasher = new();
                var usuario = new UsuarioEntity
                {
                    Username = "admin",
                    NombreCompleto = "Administrador",
                    Email = model.Correo,
                    RolId = 2 // ID del rol de administrador
                };
                usuario.PasswordHash = _hasher.HashPassword(usuario, "comunidad1234");
                nuevaComunidad.Usuarios.Add(usuario);

                await _context.Comunidades.AddAsync(nuevaComunidad);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<string>
                    {
                        Status = System.Net.HttpStatusCode.Created,
                        Message = "Comunidad creada exitosamente.",
                        Data = code
                    };
                }
                return new GenericResponse<string>
                {
                    Status = System.Net.HttpStatusCode.BadRequest,
                    Message = "No se pudo crear la comunidad.",
                    Data = ""
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<string>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = "Error al crear la comunidad.",
                    Data = ""
                };
            }
        }

        private static readonly char[] _chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789".ToCharArray();

        public string GenerateCommunityCode(int length = 6)
        {
            var random = new Random();
            var result = new char[length];

            for (int i = 0; i < length; i++)
                result[i] = _chars[random.Next(_chars.Length)];

            return new string(result);
        }
    }
}
