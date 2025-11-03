using ComuniApi.BLL.EdoCuentas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CargosController : ControllerBase
    {
        private readonly EdoCuentasService _edoCuentasService;

        public CargosController(EdoCuentasService edoCuentasService)
        {
            _edoCuentasService = edoCuentasService;
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

        [HttpGet("GetMovimientos")]
        public async Task<IActionResult> GetMovimientos()
        {
            var response = await _edoCuentasService.ObtenerMovtosPropios();
            return StatusCode((int)response.Status, response);
        }
    }
}
