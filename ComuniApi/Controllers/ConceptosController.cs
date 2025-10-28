using ComuniApi.BLL.Conceptos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConceptosController : ControllerBase
    {
        private readonly ConceptosService _conceptosService;

        public ConceptosController(ConceptosService conceptosService)
        {
            _conceptosService = conceptosService;
        }

        [HttpGet]
        public async Task<IActionResult> GetConceptos()
        {
            var response = await _conceptosService.ObtenerConceptos();
            return StatusCode((int)response.Status, response);
        }
    }
}
