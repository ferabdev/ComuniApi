using ComuniApi.BLL.Users;
using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ComuniApi.BLL.Mantenimiento
{
    public class MantenimientoService
    {
        private readonly ComuniContext _context;
        private readonly AuthService _authService;

        public MantenimientoService(ComuniContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GenericResponse<IncidenciaRes>> AgregarIncidencia(IncidenciaReq model)
        {
            try
            {
                var userId = _authService.ObtenerIdUsuario();
                var comunidadCod = _authService.ObtenerCodigoComunidad();

                if (userId == null || comunidadCod == null)
                {
                    return new GenericResponse<IncidenciaRes>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autorizado."
                    };
                }

                var comunidadId = await _context.Comunidades
                    .Where(c => c.CodigoComunidad == comunidadCod).Select(c => c.Id).FirstOrDefaultAsync();

                var incidencia = new IncidenciaEntity
                {
                    UserId = userId.Value,
                    ComunidadId = comunidadId,
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    EstatusId = 1,  //Pendiente
                    FechaRegistro = DateTime.Now
                };

                await _context.Incidencias.AddAsync(incidencia);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<IncidenciaRes>
                    {
                        Status = HttpStatusCode.Created,
                        Message = "Incidencia agregada exitosamente.",
                        Data = new IncidenciaRes
                        {
                            Id = incidencia.Id,
                            Titulo = incidencia.Titulo,
                            Descripcion = incidencia.Descripcion,
                            Estatus = "Pendiente",
                            FechaRegistro = incidencia.FechaRegistro
                        }
                    };
                }

                return new GenericResponse<IncidenciaRes>
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No se pudo agregar la incidencia."
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<IncidenciaRes>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al agregar la incidencia.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<List<IncidenciaRes>>> ObtenerIncidencias()
        {
            try
            {
                var userId = _authService.ObtenerIdUsuario();
                var comunidadCod = _authService.ObtenerCodigoComunidad();
                if (userId == null || comunidadCod == null)
                {
                    return new GenericResponse<List<IncidenciaRes>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autorizado."
                    };
                }
                var comunidadId = await _context.Comunidades
                    .Where(c => c.CodigoComunidad == comunidadCod).Select(c => c.Id).FirstOrDefaultAsync();

                var incidencias = await _context.Incidencias
                    .Where(i => i.ComunidadId == comunidadId)
                    .Select(i => new IncidenciaRes
                    {
                        Id = i.Id,
                        Usuario = i.Usuario.Username,
                        Titulo = i.Titulo,
                        Descripcion = i.Descripcion,
                        Estatus = i.Estatus.Descripcion,
                        FechaRegistro = i.FechaRegistro
                    })
                    .ToListAsync();
                return new GenericResponse<List<IncidenciaRes>>
                {
                    Status = HttpStatusCode.OK,
                    Message = "Incidencias obtenidas exitosamente.",
                    Data = incidencias
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<IncidenciaRes>>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al obtener las incidencias.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<IncidenciaRes>> ActualizarIncidencia(IncidenciaUpdReq model)
        {
            try
            {
                var comunidadCod = _authService.ObtenerCodigoComunidad();
                var userId = _authService.ObtenerIdUsuario();
                if (userId == null || comunidadCod == null)
                {
                    return new GenericResponse<IncidenciaRes>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autorizado."
                    };
                }
                var comunidadId = await _context.Comunidades
                    .Where(c => c.CodigoComunidad == comunidadCod).Select(c => c.Id).FirstOrDefaultAsync();

                var incidencia = await _context.Incidencias
                    .Where(i => i.Id == model.Id && i.ComunidadId == comunidadId).FirstOrDefaultAsync();

                if (incidencia == null)
                {
                    return new GenericResponse<IncidenciaRes>
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "Incidencia no encontrada."
                    };
                }

                incidencia.EstatusId = model.EstatusId;
                if (await _context.SaveChangesAsync() > 0)
                {
                    var estatusDesc = await _context.IncidenciasEstatus
                        .Where(e => e.Id == model.EstatusId)
                        .Select(e => e.Descripcion)
                        .FirstAsync();
                    return new GenericResponse<IncidenciaRes>
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Incidencia actualizada exitosamente.",
                        Data = new IncidenciaRes
                        {
                            Id = incidencia.Id,
                            Titulo = incidencia.Titulo,
                            Descripcion = incidencia.Descripcion,
                            Estatus = estatusDesc,
                            FechaRegistro = incidencia.FechaRegistro
                        }
                    };
                }
                return new GenericResponse<IncidenciaRes>
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No se pudo actualizar la incidencia."
                };
        }
            catch (Exception ex)
            {
                return new GenericResponse<IncidenciaRes>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al actualizar la incidencia.",
                    ExtraInfo = ex.Message
                };
            }
        }
    }
}
