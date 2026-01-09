using Microsoft.AspNetCore.Mvc;
using Social.Application.Services;

namespace Social.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly RegisterUserService _registerUserService;

    public UserController(RegisterUserService registerUserService)
    {
        _registerUserService = registerUserService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        var userId = await _registerUserService.ExecuteAsync(
            request.Email,
            request.Password
        );

        return Ok(userId);
    }
}

public record RegisterUserRequest(
    string Email,
    string Password
);
