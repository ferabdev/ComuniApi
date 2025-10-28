using ComuniApi.BLL.EdoCuentas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargosController : ControllerBase
    {
        private readonly EdoCuentasService _edoCuentasService;

        public CargosController(EdoCuentasService edoCuentasService)
        {
            _edoCuentasService = edoCuentasService;
        }

        [HttpPost("AddCargo")]
        public async Task<IActionResult> AddCargo(CargoAddReq model)
        {
            var response = await _edoCuentasService.AgregarCargo(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("AddPago")]
        public async Task<IActionResult> AddPago(PagoAddReq model)
        {
            var response = await _edoCuentasService.Agregarpago(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("GetMovimientos")]
        public async Task<IActionResult> GetMovimientos()
        {
            var response = await _edoCuentasService.ObtenerMovtosPropios();
            return StatusCode((int)response.Status, response);
        }
    }
}
