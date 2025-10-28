using ComuniApi.BLL;
using ComuniApi.BLL.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ComuniApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        //private readonly LogsService _logsService;
        private readonly AuthService _accountService;

        public AuthController(IConfiguration config, AuthService accountService)
        {
            _config = config;
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] AuthReq model)
        {
            var result = await _accountService.Login(model);
            if (result.Exitoso && result.Data != null)
            {
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, result.Data.Nombre),
                    new Claim(ClaimTypes.UserData, result.Data.Usuario),
                    new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(480),
                    signingCredentials: creds
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return StatusCode((int)result.Status, new GenericResponse<AuthRes>
                {
                    Status = HttpStatusCode.OK,
                    Message = "Sesion iniciada con exito",
                    Data = new AuthRes
                    {
                        Token = tokenString,
                        User = result.Data
                    }
                });
            }
            return StatusCode((int)result.Status, result);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserModelReq model)
        {
            var result = await _accountService.RegisterAsync(model);
            if (result.Exitoso && result.Data != null)
            {
                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
                );

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, result.Data.Nombre),
                    new Claim(ClaimTypes.UserData, result.Data.Usuario),
                    new Claim(ClaimTypes.NameIdentifier, result.Data.Id.ToString()),
                };

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(480),
                    signingCredentials: creds
                );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return StatusCode((int)result.Status, new GenericResponse<AuthRes>
                {
                    Status = HttpStatusCode.OK,
                    Message = "Sesion iniciada con exito",
                    Data = new AuthRes
                    {
                        Token = tokenString,
                        User = result.Data
                    }
                });
            }
            return StatusCode((int)result.Status, result);
        }
    }
}
