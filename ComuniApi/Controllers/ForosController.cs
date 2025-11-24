using ComuniApi.BLL.Foros;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForosController : ControllerBase
    {
        private readonly ForosService _forosService;

        public ForosController(ForosService forosService)
        {
            _forosService = forosService;
        }

        [HttpPost("IniciarForo")]
        public async Task<IActionResult> IniciarForo([FromBody] ForoReq model)
        {
            var response = await _forosService.IniciarForo(model);
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("ObtenerForos")]
        public async Task<IActionResult> ObtenerForos()
        {
            var response = await _forosService.ObtenerForos();
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("ObtenerForo/{foroId}")]
        public async Task<IActionResult> ObtenerForo(int foroId)
        {
            var response = await _forosService.ObtenerForo(foroId);
            return StatusCode((int)response.Status, response);
        }

        [HttpPost("ComentarForo")]
        public async Task<IActionResult> ComentarForo([FromBody] ComentarForoReq model)
        {
            var response = await _forosService.ComentarForo(model);
            return StatusCode((int)response.Status, response);
        }
    }
}
