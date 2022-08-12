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

    [HttpGet("tags/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var tag = await context.Tag.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (tag == null)
                return NotFound(new ResultViewModel<Tag>("Register not found."));

            return Ok(new ResultViewModel<Tag>(tag));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Tag>>("Internal error server"));
        }
    }
}