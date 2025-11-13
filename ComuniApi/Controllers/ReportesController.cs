using ComuniApi.BLL.Reportes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly ReportesService _reportesService;

        public ReportesController(ReportesService reportesService)
        {
            _reportesService = reportesService;
        }

        [HttpPost("AgregarReporte")]
        public async Task<IActionResult> AgregarReporte(ReporteReq model)
        {
            var response = await _reportesService.AgregarReporte(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("ObtenerReportes")]
        public async Task<IActionResult> ObtenerReportes()
        {
            var response = await _reportesService.ObtenerReportes();
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("ActualizarReporte")]
        public async Task<IActionResult> ActualizarReporte(ReporteUpdReq model)
        {
            var response = await _reportesService.ActualizarReporte(model);
            return StatusCode((int)response.Status, response);
        }
    }
}
