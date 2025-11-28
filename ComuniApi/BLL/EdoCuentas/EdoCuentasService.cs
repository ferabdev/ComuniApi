using ComuniApi.BLL.Users;
using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ComuniApi.BLL.EdoCuentas
{
    public class EdoCuentasService
    {
        private readonly ComuniContext _context;
        private readonly AuthService _authService;
        public EdoCuentasService(ComuniContext context, AuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GenericResponse<string>> Agregarpago(PagoAddReq model)
        {
            try
            {
                var comunidadid = _authService.ObtenerCodigoComunidad();
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == model.UsuarioId && u.Comunidad.CodigoComunidad == comunidadid);

                if (user == null)
                {
                    return new GenericResponse<string>
                    {
                        Status = System.Net.HttpStatusCode.NotFound,
                        Message = "Usuario no encontrado en la comunidad.",
                    };
                }

                var concepto = "Pago";

                var cargo = new EdoCuentaEntity
                {
                    Descripcion = concepto,
                    Monto = -model.Monto,
                    Fecha = DateTime.Now,
                    FechaLimite = model.FechaPago,
                    UsuarioId = user.Id
                };
                await _context.EdoCuentas.AddAsync(cargo);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<string>
                    {
                        Status = System.Net.HttpStatusCode.OK,
                        Message = "Pago agregado exitosamente.",
                    };
                }
                return new GenericResponse<string>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = "No se pudo agregar el pago.",
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<string>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = "Error al procesar la solicitud.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<string>> AgregarCargo(CargoAddReq model)
        {
            try
            {
                var comunidadid = _authService.ObtenerCodigoComunidad();
                var user = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == model.UsuarioId && u.Comunidad.CodigoComunidad == comunidadid);

                if (user == null)
                {
                    return new GenericResponse<string>
                    {
                        Status = System.Net.HttpStatusCode.NotFound,
                        Message = "Usuario no encontrado en la comunidad.",
                    };
                }

                var concepto = await _context.Conceptos.Where(c => c.Id == model.ConceptoId)
                    .Select(c => c.Descripcion).FirstOrDefaultAsync();

                if (concepto == null)
                {
                    return new GenericResponse<string>
                    {
                        Status = System.Net.HttpStatusCode.NotFound,
                        Message = "Concepto no encontrado.",
                    };
                }

                var cargo = new EdoCuentaEntity
                {
                    Descripcion = concepto,
                    ConceptoId = model.ConceptoId,
                    Monto = model.Monto,
                    Fecha = DateTime.Now,
                    FechaLimite = model.FechaLimite,
                    UsuarioId = user.Id
                };
                await _context.EdoCuentas.AddAsync(cargo);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<string>
                    {
                        Status = System.Net.HttpStatusCode.OK,
                        Message = "Cargo agregado exitosamente.",
                    };
                }
                return new GenericResponse<string>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = "No se pudo agregar el cargo.",
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<string>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = "Error al procesar la solicitud.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<List<MovtosRes>>> ObtenerMovtosPropios()
        {
            try
            {
                var userId = _authService.ObtenerIdUsuario();
                if (userId == null)
                {
                    return new GenericResponse<List<MovtosRes>>
                    {
                        Status = System.Net.HttpStatusCode.Unauthorized,
                        Message = "Usuario no autenticado.",
                    };
                }
                var movtos = await _context.EdoCuentas
                    .Where(e => e.UsuarioId == userId.Value)
                    .Select(e => new MovtosRes
                    {
                        Id = e.Id,
                        Concepto = e.Descripcion,
                        Monto = e.Monto,
                        Fecha = e.Fecha,
                        FechaLimite = DateOnly.FromDateTime(e.FechaLimite)
                    })
                    .ToListAsync();
                return new GenericResponse<List<MovtosRes>>
                {
                    Status = System.Net.HttpStatusCode.OK,
                    Message = "Movimientos obtenidos exitosamente.",
                    Data = movtos
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<List<MovtosRes>>
                {
                    Status = System.Net.HttpStatusCode.InternalServerError,
                    Message = "Error al procesar la solicitud.",
                    ExtraInfo = ex.Message
                };
            }
        }
    }
}
