using Blog.Data;
using Blog.Models;
using Blog.ViewModels.Errors;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Authorize]
[Route("v1")]
public class PostController : ControllerBase
{
    [HttpGet("posts")]
    public async Task<IActionResult> GetAllAsync([FromServices] BlogDataContext context)
    {
        try
        {
            var listPosts = await context.Posts
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Summary = x.Summary,
                    Slug = x.Slug,
                    Author = $"{x.Author.Name} - {x.Author.Email}",
                    Category = x.Category.Name
                })
                .ToListAsync();

            return Ok(new ResultViewModel<List<ListPostsViewModel>>(listPosts));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Any value null on object."));
        }
    }
}