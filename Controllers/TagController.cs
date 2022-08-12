using Blog.Data;
using Blog.Models;
using Blog.ViewModels.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Authorize]
[Route("v1")]
public class TagController : ControllerBase
{
    [HttpGet("tags")]
    public async Task<IActionResult> Get([FromServices] BlogDataContext context)
    {
        try
        {
            var listTags = await context.Tag.AsNoTracking().ToListAsync();

            return Ok(new ResultViewModel<List<Tag>>(listTags));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("Any value null on object."));
        }
    }
}