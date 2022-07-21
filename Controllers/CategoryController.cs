﻿using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels.Categories;
using Blog.ViewModels.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Route("v1")]
[Authorize]
public class CategoryController : ControllerBase
{
    [HttpGet("categories")]
    public async Task<IActionResult> Get([FromServices] BlogDataContext context)
    {
        try
        {
            var listCategory = await context.Categories.AsNoTracking().ToListAsync();

            return Ok(new ResultViewModel<List<Category>>(listCategory));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Any value null on object."));
        }
    }

    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new ResultViewModel<Category>("Register not found."));

            return Ok(new ResultViewModel<Category>(category));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Category>>("Internal error server"));
        }
    }

    [HttpPost("categories")]
    public async Task<IActionResult> Post([FromServices] BlogDataContext context, [FromBody] EditorCategoryViewModel category)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

        try
        {
            var categoryObject = new Category()
            {
                Name = category.Name,
                Slug = category.Slug
            };

            await context.Categories.AddAsync(categoryObject);
            await context.SaveChangesAsync();

            return Created($"categories/{categoryObject.Id}", categoryObject);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>( "Failed to insert register."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("Internal error server"));
        }
    }

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> Put([FromServices] BlogDataContext context, [FromBody] EditorCategoryViewModel category, [FromRoute] int id)
    {
        try
        {
            var register = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (register == null)
                return NotFound(new ResultViewModel<Category>(ModelState.GetErrors()));

            register.Name = category.Name;
            register.Slug = category.Slug;

            context.Categories.Update(register);
            await context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("Failed to insert register."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("Internal error server"));
        }
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> Delete([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var register = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (register == null)
                return NotFound(new ResultViewModel<Category>(ModelState.GetErrors()));

            context.Categories.Remove(register);
            await context.SaveChangesAsync();

            return Ok(register);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Category>("Failed to insert register."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Category>("Internal error server"));
        }
    }
}
