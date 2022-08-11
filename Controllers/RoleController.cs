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
}