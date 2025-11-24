using ComuniApi.BLL.Users;
using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ComuniApi.BLL.Foros
{
    public class ForosService
    {
        private readonly ComuniContext _context;
        private readonly AuthService _authService;

        public ForosService(ComuniContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GenericResponse<int>> IniciarForo(ForoReq model)
        {
            try
            {
                var usuarioId = _authService.ObtenerIdUsuario();
                var comunidadCod = _authService.ObtenerCodigoComunidad();

                if (usuarioId == null)
                {
                    return new GenericResponse<int>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autenticado."
                    };
                }

                if (comunidadCod == null)
                {
                    return new GenericResponse<int>
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message = "Comunidad no encontrada."
                    };
                }

                var comunidad = await _context.Comunidades
                    .FirstAsync(c => c.CodigoComunidad == comunidadCod);

                var foro = new ForoEntity
                {
                    Nombre = model.Nombre,
                    FechaCreacion = DateTime.Now,
                    ComunidadId = comunidad.Id,
                    Comentarios = model.Comentarios.Select(c => new ForoComentarioEntity
                    {
                        UsuarioId = usuarioId.Value,
                        Mensaje = c.Mensaje,
                        Fecha = DateTime.Now
                    }).ToList()
                };

                await _context.Foros.AddAsync(foro);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<int>
                    {
                        Status = HttpStatusCode.Created,
                        Message = "Foro iniciado exitosamente.",
                        Data = foro.Id
                    };
                }
                else
                {
                    return new GenericResponse<int>
                    {
                        Status = HttpStatusCode.InternalServerError,
                        Message = "No se pudo iniciar el foro."
                    };
                }

            }
            catch (Exception ex)
            {
                return new GenericResponse<int>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Ocurrio un error al iniciar el foro.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<List<ForoRes>>> ObtenerForos()
        {
            try
            {
                var comunidadCod = _authService.ObtenerCodigoComunidad();
                if (comunidadCod == null)
                {
                    return new GenericResponse<List<ForoRes>>
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message = "Comunidad no encontrada."
                    };
                }
                var foros = await _context.Foros
                    .Where(f => f.Comunidad.CodigoComunidad == comunidadCod)
                    .Include(f => f.Comentarios)
                    .ThenInclude(c => c.Usuario)
                    .ToListAsync();
                var result = foros.Select(f => new ForoRes
                {
                    Id = f.Id,
                    Nombre = f.Nombre,
                    FechaCreacion = f.FechaCreacion,
                    Usuario = f.Comentarios.FirstOrDefault()?.Usuario.Username ?? "Desconocido",
                    Comentarios = new List<ForoComentarioRes>()
                }).ToList();
                return new GenericResponse<List<ForoRes>>
                {
                    Status = HttpStatusCode.OK,
                    Message = "Foros obtenidos exitosamente.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<ForoRes>>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Ocurrio un error al obtener los foros.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<ForoRes>> ObtenerForo(int id)
        {
            try
            {
                var result = await _context.Foros
                    .Where(f => f.Id == id)
                    .Select(f => new ForoRes
                    {
                        Id = f.Id,
                        Nombre = f.Nombre,
                        FechaCreacion = f.FechaCreacion,
                        Usuario = f.Comentarios.First().Usuario.Username ?? "Desconocido",
                        
                    })
                    .FirstOrDefaultAsync();
                if (result == null)
                {
                    return new GenericResponse<ForoRes>
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "Foro no encontrado."
                    };
                }

                result.Comentarios = await _context.ForoComentarios
                    .Where(c => c.ForoId == id)
                    .Include(c => c.Usuario)
                    .Select(c => new ForoComentarioRes
                    {
                        Id = c.Id,
                        Mensaje = c.Mensaje,
                        Fecha = c.Fecha,
                        Usuario = c.Usuario.Username ?? "Desconocido"
                    })
                    .ToListAsync();

                return new GenericResponse<ForoRes>
                {
                    Status = HttpStatusCode.OK,
                    Message = "Foro obtenido exitosamente.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<ForoRes>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Ocurrio un error al obtener el foro.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<string>> ComentarForo(ComentarForoReq model)
        {
            try
            {
                var usuarioId = _authService.ObtenerIdUsuario();
                if (usuarioId == null)
                {
                    return new GenericResponse<string>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autenticado."
                    };
                }

                var foro = await _context.Foros.FindAsync(model.ForoId);
                if (foro == null)
                {
                    return new GenericResponse<string>
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "Foro no encontrado."
                    };
                }

                var comentario = new ForoComentarioEntity
                {
                    ForoId = model.ForoId,
                    UsuarioId = usuarioId.Value,
                    Mensaje = model.Mensaje,
                    Fecha = DateTime.Now
                };

                await _context.ForoComentarios.AddAsync(comentario);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<string>
                    {
                        Status = HttpStatusCode.Created,
                        Message = "Comentario agregado exitosamente."
                    };
                }

                return new GenericResponse<string>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "No se pudo agregar el comentario."
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<string>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Ocurrio un error al comentar el foro.",
                    ExtraInfo = ex.Message
                };
            }
        }
    }
}
