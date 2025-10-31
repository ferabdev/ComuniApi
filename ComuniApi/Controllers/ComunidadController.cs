using ComuniApi.BLL.Comunidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComunidadController : ControllerBase
    {
        private readonly ComunidadService _comunidadService;

        public ComunidadController(ComunidadService comunidadService)
        {
            _comunidadService = comunidadService;
        }

        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var comunidadIdClaim = User.Claims.FirstOrDefault(c => c.Type == "ComunidadId");
            var comunidadId = comunidadIdClaim != null ? int.Parse(comunidadIdClaim.Value) : 0;

            var response = await _comunidadService.ObtenerUsuariosComunidad(comunidadId);
            return StatusCode((int)response.Status, response);
        }
    }
}
