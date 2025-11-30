using ComuniApi.BLL.Mantenimiento;
using ComuniApi.Controllers;
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
        private readonly IncidenciasService _incidenciaService;

        public MantenimientoController(IncidenciasService incidenciasService)
        {
            _incidenciaService = incidenciasService;
        }

        [HttpPost("AgregarIncidencia")]
        //[Authorize(Roles = Constantes.Roles.Administrador)]
        public async Task<IActionResult> AgregarIncidencia(IncidenciaReq model)
        {
            var response = await _incidenciaService.AgregarIncidencia(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("ObtenerIncidencias")]
        public async Task<IActionResult> ObtenerIncidencias()
        {
            var response = await _incidenciaService.ObtenerIncidencias();
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("ActualizarIncidencia")]
        [Authorize(Roles = Constantes.Roles.Administrador)]
        public async Task<IActionResult> ActualizarIncidencia(IncidenciaUpdReq model)
        {
            var response = await _incidenciaService.ActualizarIncidencia(model);
            return StatusCode((int)response.Status, response);
        }
    }
}
