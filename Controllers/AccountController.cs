using Blog.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class AccountController : ControllerBase
{
    [HttpPost("token")]
    public IActionResult Token([FromServices] TokenService tokenService)
    {   
        var token = tokenService.GenerateToken(null);

        return Ok(token);
    }
}

