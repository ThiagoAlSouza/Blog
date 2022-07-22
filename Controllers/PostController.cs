using Blog.Data;
using Blog.Models;
using Blog.ViewModels.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
public class PostController : ControllerBase
{
    [HttpGet("posts")]
    public async Task<IActionResult> GetAllAsync([FromServices] BlogDataContext context)
    {
        try
        {
            var listPosts = await context.Posts.AsNoTracking().ToListAsync();

            return Ok(new ResultViewModel<List<Post>>(listPosts));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Any value null on object."));
        }
    }
}