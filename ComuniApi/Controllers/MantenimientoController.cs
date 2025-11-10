using ComuniApi.BLL.Mantenimiento;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MantenimientoController : ControllerBase
    {
        private readonly MantenimientoService _mantenimientoService;

        public MantenimientoController(MantenimientoService mantenimientoService)
        {
            _mantenimientoService = mantenimientoService;
        }

        [HttpPost("AgregarIncidencia")]
        [Authorize(Roles = Constantes.Roles.Administrador)]
        public async Task<IActionResult> AgregarIncidencia(IncidenciaReq model)
        {
            var response = await _mantenimientoService.AgregarIncidencia(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("ObtenerIncidencias")]
        public async Task<IActionResult> ObtenerIncidencias()
        {
            var response = await _mantenimientoService.ObtenerIncidencias();
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("ActualizarIncidencia")]
        [Authorize(Roles = Constantes.Roles.Administrador)]
        public async Task<IActionResult> ActualizarIncidencia(IncidenciaUpdReq model)
        {
            var response = await _mantenimientoService.ActualizarIncidencia(model);
            return StatusCode((int)response.Status, response);
        }
    }
}
