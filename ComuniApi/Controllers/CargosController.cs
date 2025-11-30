using ComuniApi.BLL.EdoCuentas;
using ComuniApi.BLL.PagosOnline;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CargosController : ControllerBase
    {
        private readonly EdoCuentasService _edoCuentasService;
        private readonly PagoOnlineService _pagoOnlineService;

        public CargosController(EdoCuentasService edoCuentasService, PagoOnlineService pagoOnlineService)
        {
            _edoCuentasService = edoCuentasService;
            _pagoOnlineService = pagoOnlineService;
        }

        [Authorize(Roles = Constantes.Roles.Administrador)]
        [HttpPost("AddCargo")]
        public async Task<IActionResult> AddCargo(CargoAddReq model)
        {
            var response = await _edoCuentasService.AgregarCargo(model);
            return StatusCode((int)response.Status, response);
        }

        [Authorize(Roles = Constantes.Roles.Administrador)]
        [HttpPost("AddPago")]
        public async Task<IActionResult> AddPago(PagoAddReq model)
        {
            var response = await _edoCuentasService.Agregarpago(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("GenerarEnlacePago")]
        public async Task<IActionResult> GenerarEnlacePago(PagoOnlineReq model)
        {
            var response = await _pagoOnlineService.GenerarEnlacePago(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("GetMovimientos")]
        public async Task<IActionResult> GetMovimientos()
        {
            var response = await _edoCuentasService.ObtenerMovtosPropios();
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("pagoWebhook")]
        [SwaggerIgnore]
        [AllowAnonymous]
        public async Task<IActionResult> WeebhookPagos()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                if (stripeEvent.Type == EventTypes.CheckoutSessionAsyncPaymentSucceeded || stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    using var doc = JsonDocument.Parse(json);

                    var root = doc.RootElement;

                    // Ruta:
                    // data → object → client_reference_id
                    var clientRef = root
                        .GetProperty("data")
                        .GetProperty("object")
                        .GetProperty("client_reference_id")
                        .GetString();

                    //var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    //var referenceID = session!.ClientReferenceId;
                    await _pagoOnlineService.ConfirmarPago(int.Parse(clientRef));
                }

                return Ok();
            }
            catch (StripeException)
            {
                return BadRequest();
            }
        }
    }
}
