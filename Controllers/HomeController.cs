using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class HomeController : ControllerBase
{
    [HttpGet("")]
    public IActionResult Get()
    {
        return Ok();
    }
}
