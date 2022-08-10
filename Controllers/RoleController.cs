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
public class RoleController : ControllerBase
{
    [HttpGet]
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
}