using ComuniApi.BLL.Users;
using ComuniApi.DAL;
using ComuniApi.DAL.Entidades;
using Stripe;
using Stripe.Checkout;
using System.Net;

namespace ComuniApi.BLL.PagosOnline
{
    public class PagoOnlineService
    {
        private readonly ComuniContext _context;
        private readonly AuthService _authService;
        private readonly IConfiguration _config;

        public PagoOnlineService(ComuniContext context, AuthService authService, IConfiguration config)
        {
            _context = context;
            _authService = authService;
            _config = config;
        }

        public async Task<GenericResponse<string>> GenerarEnlacePago(PagoOnlineReq model)
        {
            try
            {
                var userid = _authService.ObtenerIdUsuario();
                if (userid == null)
                {
                    return new GenericResponse<string>
                    {
                        Status = HttpStatusCode.Unauthorized,
                        Message = "Usuario no autenticado.",
                    };
                }

                var concepto = "Pago";

                var cargo = new EdoCuentaEntity
                {
                    Descripcion = concepto,
                    Monto = -model.MontoPago,
                    Fecha = DateTime.Now,
                    FechaLimite = DateTime.Now,
                    UsuarioId = (int)userid,
                    EstatusId = 1
                };
                await _context.EdoCuentas.AddAsync(cargo);

                if (await _context.SaveChangesAsync() > 0)
                {
                    var options = new SessionCreateOptions
                    {
                        LineItems = new List<SessionLineItemOptions>
                        {
                            new SessionLineItemOptions
                            {
                                PriceData = new SessionLineItemPriceDataOptions
                                {
                                    Currency = "mxn",
                                    ProductData = new SessionLineItemPriceDataProductDataOptions
                                    {
                                        Name = "Pago en línea a comunidad",
                                    },
                                    UnitAmount = (long)(model.MontoPago * 100), // Monto en centavos
                                },
                                Quantity = 1,
                            },
                        },
                        Mode = "payment",
                        SuccessUrl = "comuniapp://pago-exitoso" + "?success=true",
                        ClientReferenceId = cargo.Id.ToString(),
                    };
                    StripeClient client = new StripeClient(_config["Stripe:SecretKey"]);
                    var service = new SessionService(client);
                    Session session = await service.CreateAsync(options);

                    return new GenericResponse<string>
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Enlace de pago generado exitosamente.",
                        Data = session.Url
                    };
                }

                return new GenericResponse<string>
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No se pudo generar el enlace de pago.",
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<string>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al generar el enlace de pago.",
                    ExtraInfo = ex.Message
                };
            }
        }

        public async Task<GenericResponse<int>> ConfirmarPago(int pagoId)
        {
            try
            {
                var pago = await _context.EdoCuentas.FindAsync(pagoId);
                if (pago == null)
                {
                    return new GenericResponse<int>
                    {
                        Status = HttpStatusCode.NotFound,
                        Message = "Pago no encontrado.",
                    };
                }

                pago.EstatusId = 2;
                _context.EdoCuentas.Update(pago);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return new GenericResponse<int>
                    {
                        Status = HttpStatusCode.OK,
                        Message = "Pago confirmado exitosamente.",
                        Data = pago.Id
                    };
                }
                return new GenericResponse<int>
                {
                    Status = HttpStatusCode.BadRequest,
                    Message = "No se pudo confirmar el pago.",
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse<int>
                {
                    Status = HttpStatusCode.InternalServerError,
                    Message = "Error al confirmar el pago.",
                    ExtraInfo = ex.Message
                };
            }
        }
    }
}
