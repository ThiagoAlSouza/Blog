﻿using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels.Errors;
using Blog.ViewModels.Tags;
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

    [HttpPost("tags")]
    public async Task<IActionResult> Post([FromServices] BlogDataContext context, [FromBody] EditorTagViewModel? body)
    {
        try
        {
            if (body == null)
                return BadRequest(new ResultViewModel<Tag>(ModelState.GetErrors()));

            var tag = new Tag
            {
                Name = body.Name,
                Slug = body.Slug
            };

            await context.Tag.AddAsync(tag);
            await context.SaveChangesAsync();

            return Created($"tags/{tag.Id}", tag);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Tag>("Failed to insert register."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Tag>("Internal error server"));
        }
    }

    [HttpPut("tags/{id:int}")]
    public async Task<IActionResult> Put([FromServices] BlogDataContext context, [FromBody] EditorTagViewModel body, [FromRoute] int id)
    {
        try
        {
            var register = await context.Tag.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (register == null)
                return NotFound(new ResultViewModel<Tag>(ModelState.GetErrors()));

            register.Name = body.Name;
            register.Slug = body.Slug;

            context.Tag.Update(register);
            await context.SaveChangesAsync();

            return Ok(register);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Tag>("Failed to insert register."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Tag>("Internal error server"));
        }
    }

    [HttpDelete("tags/{id:int}")]
    public async Task<IActionResult> Delete([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var register = await context.Tag.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (register == null)
                return NotFound(new ResultViewModel<Tag>(ModelState.GetErrors()));

            context.Tag.Remove(register);
            await context.SaveChangesAsync();

            return Ok(register);
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Tag>("Failed to insert register."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Tag>("Internal error server"));
        }
    }
}