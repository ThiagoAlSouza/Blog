using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels.Errors;
using Blog.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[ApiController]
[Authorize]
[Route("v1")]
public class RoleController : ControllerBase
{
    [HttpGet("roles")]
    public async Task<IActionResult> Get([FromServices] BlogDataContext context)
    {
        try
        {
            var listRole = await context.Role.AsNoTracking().ToListAsync();

            return Ok(new ResultViewModel<List<Role>>(listRole));
        }
        catch (ArgumentNullException)
        {
            return StatusCode(500, new ResultViewModel<List<Role>>("Any value null on object."));
        }
    }

    [HttpGet("roles/{id:int}")]
    public async Task<IActionResult> GetByIdAsync([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var role = await context.Role.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (role == null)
                return NotFound(new ResultViewModel<Role>("Register not found."));

            return Ok(new ResultViewModel<Role>(role));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<Role>>("Internal error server"));
        }
    }

    [HttpPost("roles")]
    public async Task<IActionResult> Post([FromServices] BlogDataContext context, [FromBody] EditorRoleViewModel body)
    {
        try
        {
            if (body == null)
                return BadRequest(new ResultViewModel<Role>(ModelState.GetErrors()));

            var role = new Role
            {
                Name = body.Name,
                Slug = body.Slug
            };

            await context.Role.AddAsync(role);
            await context.SaveChangesAsync();

            return Created($"roles/{role.Id}", role);
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

    [HttpPut("roles/{id:int}")]
    public async Task<IActionResult> Put([FromServices] BlogDataContext context, [FromBody] EditorRoleViewModel body, [FromRoute] int id)
    {
        try
        {
            var register = await context.Role.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (register == null)
                return NotFound(new ResultViewModel<Category>(ModelState.GetErrors()));

            register.Name = body.Name;
            register.Slug = body.Slug;

            context.Role.Update(register);
            await context.SaveChangesAsync();

            return Ok();
        }
        catch (DbUpdateException)
        {
            return StatusCode(500, new ResultViewModel<Role>("Failed to insert register."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<Role>("Internal error server"));
        }
    }

}