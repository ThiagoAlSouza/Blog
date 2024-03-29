﻿using Blog.Data;
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
    public async Task<IActionResult> GetAllAsync([FromServices] BlogDataContext context, [FromQuery]int page = 0, [FromQuery] int pageSize = 25)
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
                    Author = x.Author,
                    Category = x.Category
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(new ResultViewModel<List<ListPostsViewModel>>(listPosts));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Post>>("Any value null on object."));
        }
    }

    [HttpGet("posts/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, int id)
    {
        try
        {
            var post = await context.Posts 
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .ThenInclude(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (post == null)
                return NotFound(new ResultViewModel<Post>("Register not found."));

            return Ok(new ResultViewModel<Post>(post));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Post>>("Any value null on object."));
        }
    }

    [HttpGet("posts/category/{category}")]
    public async Task<IActionResult> GetByCategoryAsync([FromServices] BlogDataContext context, [FromRoute] string category, [FromQuery] int page = 0, [FromQuery] int pageSize = 25)
    {
        try
        {
            var listPosts = await context.Posts
                .AsNoTracking()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Where(x => x.Category.Slug == category)
                .Select(x => new ListPostsViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Summary = x.Summary,
                    Slug = x.Slug,
                    Author = x.Author,
                    Category = x.Category
                })
                .Skip(page * pageSize)
                .Take(pageSize)
                .OrderByDescending(x => x.Id)
                .ToListAsync();

            return Ok(new ResultViewModel<List<ListPostsViewModel>>(listPosts));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Post>>("Any value null on object."));
        }
    }
}