using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosOnlineController : ControllerBase
    {
        [HttpPost("Suscripcion")]
        public async Task<IActionResult> Suscripcion()
        {

            // Placeholder for future implementation
            return StatusCode(StatusCodes.Status501NotImplemented, new
            {
                Status = StatusCodes.Status501NotImplemented,
                Message = "Not implemented yet"
            });
        }
    }
}
