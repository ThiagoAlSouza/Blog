using System.Linq.Expressions;
using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
public class CategoryController : ControllerBase
{
    [HttpGet("categories")]
    public async Task<IActionResult> Get([FromServices] BlogDataContext context)
    {
        var listCategory = await context.Categories.AsNoTracking().ToListAsync();

        return Ok(listCategory);
    }

    [HttpGet("categories/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (category == null)
            return NotFound();

        return Ok(category);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> Post([FromServices] BlogDataContext context, [FromBody] Category category) 
    {
        if (category == null)
            return BadRequest();

        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();

        return Created($"categories/{category.Id}", category);
    }

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> Put([FromServices] BlogDataContext context, [FromBody] Category category, [FromRoute] int id)
    {
        if (category == null)
            return BadRequest();

        var register = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        
        if(register == null)
            return NotFound();

        register.Name = category.Name;
        register.Slug = category.Slug;

        context.Categories.Update(register);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> Delete([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        var register = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (register == null)
            return NotFound();
        
        context.Categories.Remove(register);
        await context.SaveChangesAsync();

        return Ok(register);
    }
}
