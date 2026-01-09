using Microsoft.AspNetCore.Mvc;
using Social.Application.Services;

namespace Social.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly LoginUserService _loginUserService;

        public AuthController(LoginUserService loginUserService)
        {
            _loginUserService = loginUserService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var token = await _loginUserService.ExecuteAsync(request.Email, request.Password);
            return Ok(new { Token = token });
        }
    }

    public record LoginRequest(string Email, string Password);
}
