using Blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[Authorize]
[ApiController]
[Route("v1")]
public class AccountController : ControllerBase
{
    [HttpGet("user")]
    public IActionResult GetUser()
    {
        return Ok(User.Identity.Name);
    }

    [HttpGet("author")]
    public IActionResult GetAuthor()
    {
        return Ok(User.Identity.Name);
    }

    [HttpGet("admin")]
    public IActionResult GeAdmin()
    {
        return Ok(User.Identity.Name);
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult Token([FromServices] TokenService tokenService)
    {   
        var token = tokenService.GenerateToken(null);

        return Ok(token);
    }
}