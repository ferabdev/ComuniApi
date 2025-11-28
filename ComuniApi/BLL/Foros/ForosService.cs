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
                    UsuarioId = usuarioId.Value,
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
                    .Include(f => f.Usuario)
                    .Include(f => f.Opciones)
                    .ToListAsync();
                var result = foros.Select(f => new ForoRes
                {
                    Id = f.Id,
                    Nombre = f.Nombre,
                    FechaCreacion = f.FechaCreacion,
                    Usuario = f.Usuario.Username,
                    Comentarios = new List<ForoComentarioRes>(),
                    EsVotacion = f.Votacion,
                    Opciones = f.Opciones.Select(o => new VotacionOpcionRes
                    {
                        OpcionId = o.Id,
                        Descripcion = o.Descripcion,
                        Votos = o.Votos
                    }).ToList()
                }).ToList();

                //result.Where(f => f.EsVotacion).ToList().ForEach(votacion =>
                //{
                //    votacion.Opciones = _context.ForoVotacionOpciones
                //        .Where(o => o.ForoId == votacion.Id)
                //        .Select(o => new VotacionOpcionRes
                //        {
                //            OpcionId = o.Id,
                //            Descripcion = o.Descripcion,
                //            Votos = o.Votos
                //        })
                //        .ToListAsync();
                //});

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
                        Usuario = f.Usuario.Username,
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

        public async Task<GenericResponse<int>> CrearVotacion(VotacionReq model)
        {
            try
            {
                var usuarioId = _authService.ObtenerIdUsuario();
                if (usuarioId == null) return new GenericResponse<int>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Message = "Usuario no autenticado."
                };
                var comunidadId = await _context.Comunidades
                    .Where(c => c.CodigoComunidad == _authService.ObtenerCodigoComunidad())
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();

                var foroVotacion = new ForoEntity
                {
                    Nombre = model.Asunto,
                    FechaCreacion = DateTime.Now,
                    ComunidadId  = comunidadId,
                    Votacion = true,
                    UsuarioId = usuarioId.Value,
                    Opciones = model.Opciones.Select(o => new ForoVotacionOpcionEntity
                    {
                        Descripcion = o
                    }).ToList()
                };

                await _context.Foros.AddAsync(foroVotacion);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<int>
                    {
                        Status = HttpStatusCode.Created,
                        Message = "Votacion creada exitosamente.",
                        Data = foroVotacion.Id
                    };
                }

                return new GenericResponse<int>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "No se pudo crear la votacion."
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<int>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Ocurrio un error al crear la votacion.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<string>> VotarEnVotacion(VotarReq model)
        {
            try
            {
                var usuarioId = _authService.ObtenerIdUsuario();
                if (usuarioId == null) return new GenericResponse<string>
                {
                    Status = HttpStatusCode.Unauthorized,
                    Message = "Usuario no autenticado."
                };

                var votoExistente = await _context.ForosVotosUsuarios
                    .FirstOrDefaultAsync(v => v.ForoId == model.VotacionId && v.UsuarioId == usuarioId.Value);
                if (votoExistente != null)
                {
                    await _context.ForoVotacionOpciones.Where(o => o.Id == votoExistente.OpcionId)
                        .ExecuteUpdateAsync(u => u
                            .SetProperty(u => u.Votos, u => u.Votos - 1)
                        );
                    _context.ForosVotosUsuarios.Remove(votoExistente);
                }

                var opcion = await _context.ForoVotacionOpciones
                    .FirstOrDefaultAsync(o => o.Id == model.OpcionId && o.ForoId == model.VotacionId);
                if (opcion == null)
                {
                    return new GenericResponse<string>
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "Opcion de votacion no encontrada."
                    };
                }
                opcion.Votos += 1;
                var votoUsuario = new ForoVotoUsuarioEntity
                {
                    ForoId = model.VotacionId,
                    UsuarioId = usuarioId.Value,
                    OpcionId = model.OpcionId
                };
                await _context.ForosVotosUsuarios.AddAsync(votoUsuario);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<string>
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Voto registrado exitosamente."
                    };
                }
                return new GenericResponse<string>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "No se pudo registrar el voto."
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<string>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Ocurrio un error al registrar el voto.",
                    ExtraInfo = ex.Message
                };
            }
        }

        //Obtener votaciones
        public async Task<GenericResponse<List<VotacionRes>>> ObtenerVotaciones()
        {
            try
            {
                var comunidadCod = _authService.ObtenerCodigoComunidad();
                if (comunidadCod == null)
                {
                    return new GenericResponse<List<VotacionRes>>
                    {
                        Status = HttpStatusCode.BadRequest,
                        Message = "Comunidad no encontrada."
                    };
                }

                var votaciones = await _context.Foros
                    .Where(f => f.Comunidad.CodigoComunidad == comunidadCod && f.Votacion)
                    .Include(f => f.Opciones)
                    .ToListAsync();
                var result = votaciones.Select(f => new VotacionRes
                {
                    Id = f.Id,
                    Asunto = f.Nombre,
                    FechaCreacion = f.FechaCreacion,
                    Usuario = f.Usuario.Username,
                    Opciones = f.Opciones.Select(o => new VotacionOpcionRes
                    {
                        OpcionId = o.Id,
                        Descripcion = o.Descripcion,
                        Votos = o.Votos
                    }).ToList()
                    //PENDIENTE LAS OPCIONES
                }).ToList();

                return new GenericResponse<List<VotacionRes>>
                {
                    Status = HttpStatusCode.OK,
                    Message = "Votaciones obtenidas exitosamente.",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<VotacionRes>>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Ocurrio un error al obtener las votaciones.",
                    ExtraInfo = ex.Message
                };
            }
        }
    }
}
