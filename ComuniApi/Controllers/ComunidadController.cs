using ComuniApi.BLL.Comunidades;
using ComuniApi.BLL.Users;
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
        private readonly AuthService _authService;

        public ComunidadController(ComunidadService comunidadService, AuthService authService)
        {
            _comunidadService = comunidadService;
            _authService = authService;
        }

        [HttpGet("ObtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuarios()
        {
            var comunidadId = _authService.ObtenerComunidadId();

            var response = await _comunidadService.ObtenerUsuariosComunidad(comunidadId);
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("CrearComunidad")]
        [Authorize(Roles = Constantes.Roles.SuperAdmin)]
        public async Task<IActionResult> CrearComunidad(ComunidadReq model)
        {
            var response = await _comunidadService.CrearComunidad(model);
            return StatusCode((int)response.Status, response);
        }
    }
}
