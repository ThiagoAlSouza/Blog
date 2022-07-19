using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers;

[Authorize]
[ApiController]
[Route("v1")]
public class AccountController : ControllerBase
{
    [HttpGet("account/{id:int}")]
    public async Task<IActionResult> GetById([FromServices] BlogDataContext context, [FromRoute] int id)
    {
        try
        {
            var register = await context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (register == null)
                return NotFound(new ResultViewModel<User>("Register not found."));

            return Ok(new ResultViewModel<User>(register));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<List<User>>("Internal error server"));
        }
    }

    [HttpPost("account")]
    public async Task<IActionResult> CreateAccount([FromServices] BlogDataContext context, [FromBody] RegisterUserViewModel userView)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<RegisterUserViewModel>(ModelState.GetErrors()));

            var user = new User
            {
                Name = userView.Name,
                Email = userView.email,
                Slug = userView.email.Replace("@", "-").Replace(".", "-"),
                PasswordHash = userView.senha
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Created("$account/{user.id}", user);
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<RegisterUserViewModel>(ModelState.GetErrors()));
        }
    }

    [AllowAnonymous]
    [HttpPost("token")]
    public IActionResult Token([FromServices] TokenService tokenService)
    {   
        var token = tokenService.GenerateToken(null);

        return Ok(token);
    }
}