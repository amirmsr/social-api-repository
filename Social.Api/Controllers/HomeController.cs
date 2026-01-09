using Microsoft.AspNetCore.Mvc;

namespace Social.Api.Controllers;

[ApiController]
[Route("/")] // route racine
public class HomeController : ControllerBase
{
    [HttpGet]
    public IActionResult Hello()
    {
        return Ok("Hello world 👋");
    }
}
