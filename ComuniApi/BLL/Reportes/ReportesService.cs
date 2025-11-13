using ComuniApi.BLL.Users;
using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace ComuniApi.BLL.Reportes
{
    public class ReportesService
    {
        private readonly ComuniContext _context;
        private readonly AuthService _authService;

        public ReportesService(ComuniContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GenericResponse<ReporteRes>> AgregarReporte(ReporteReq model)
        {
            try
            {
                var userId = _authService.ObtenerIdUsuario();
                var comunidadCod = _authService.ObtenerCodigoComunidad();

                if (userId == null || comunidadCod == null)
                {
                    return new GenericResponse<ReporteRes>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autorizado."
                    };
                }

                var comunidadId = await _context.Comunidades
                    .Where(c => c.CodigoComunidad == comunidadCod).Select(c => c.Id).FirstOrDefaultAsync();

                var reporte = new ReporteEntity
                {
                    UserId = userId.Value,
                    Titulo = model.Titulo,
                    Descripcion = model.Descripcion,
                    EstatusId = 1,  //Pendiente
                    FechaRegistro = DateTime.Now
                };

                await _context.Reportes.AddAsync(reporte);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<ReporteRes>
                    {
                        Status = HttpStatusCode.Created,
                        Message = "Reporte agregado exitosamente.",
                        Data = new ReporteRes
                        {
                            Id = reporte.Id,
                            Titulo = reporte.Titulo,
                            Descripcion = reporte.Descripcion,
                            Estatus = "Pendiente",
                            FechaRegistro = reporte.FechaRegistro
                        }
                    };
                }

                return new GenericResponse<ReporteRes>
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No se pudo agregar el reporte."
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<ReporteRes>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al agregar el reporte.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<List<ReporteRes>>> ObtenerReportes()
        {
            try
            {
                var userId = _authService.ObtenerIdUsuario();
                var comunidadCod = _authService.ObtenerCodigoComunidad();
                if (userId == null || comunidadCod == null)
                {
                    return new GenericResponse<List<ReporteRes>>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autorizado."
                    };
                }
                var comunidadId = await _context.Comunidades
                    .Where(c => c.CodigoComunidad == comunidadCod).Select(c => c.Id).FirstOrDefaultAsync();

                var reportes = await _context.Reportes
                    .Where(i => i.Usuario.ComunidadId == comunidadId)
                    .Select(i => new ReporteRes
                    {
                        Id = i.Id,
                        Usuario = i.Usuario.Username,
                        Titulo = i.Titulo,
                        Descripcion = i.Descripcion,
                        Estatus = i.Estatus.Descripcion,
                        FechaRegistro = i.FechaRegistro
                    })
                    .ToListAsync();
                return new GenericResponse<List<ReporteRes>>
                {
                    Status = HttpStatusCode.OK,
                    Message = "Reportes obtenidos exitosamente.",
                    Data = reportes
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<ReporteRes>>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al obtener los reportes.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<ReporteRes>> ActualizarReporte(ReporteUpdReq model)
        {
            try
            {
                var comunidadCod = _authService.ObtenerCodigoComunidad();
                var userId = _authService.ObtenerIdUsuario();
                if (userId == null || comunidadCod == null)
                {
                    return new GenericResponse<ReporteRes>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autorizado."
                    };
                }
                var comunidadId = await _context.Comunidades
                    .Where(c => c.CodigoComunidad == comunidadCod).Select(c => c.Id).FirstOrDefaultAsync();

                var reporte = await _context.Reportes
                    .Where(i => i.Id == model.Id && i.Usuario.ComunidadId == comunidadId).FirstOrDefaultAsync();

                if (reporte == null)
                {
                    return new GenericResponse<ReporteRes>
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "Reporte no encontrado."
                    };
                }

                reporte.EstatusId = model.EstatusId;
                if (await _context.SaveChangesAsync() > 0)
                {
                    var estatusDesc = await _context.ReportesEstatus
                        .Where(e => e.Id == model.EstatusId)
                        .Select(e => e.Descripcion)
                        .FirstAsync();
                    return new GenericResponse<ReporteRes>
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Reporte actualizado exitosamente.",
                        Data = new ReporteRes
                        {
                            Id = reporte.Id,
                            Titulo = reporte.Titulo,
                            Descripcion = reporte.Descripcion,
                            Estatus = estatusDesc,
                            FechaRegistro = reporte.FechaRegistro
                        }
                    };
                }
                return new GenericResponse<ReporteRes>
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No se pudo actualizar el reporte."
                };
        }
            catch (Exception ex)
            {
                return new GenericResponse<ReporteRes>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al actualizar el reporte.",
                    ExtraInfo = ex.Message
                };
            }
        }
    }
}
