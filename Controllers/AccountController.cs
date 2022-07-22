using System.Text.RegularExpressions;
using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels.Accounts;
using Blog.ViewModels.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

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
    public async Task<IActionResult> CreateAccount([FromServices] BlogDataContext context, [FromBody] RegisterUserViewModel model, [FromServices] EmailService emailService)
    {
        try
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<RegisterUserViewModel>(ModelState.GetErrors()));

            var passwordGuid = Guid.NewGuid().ToString();

            var user = new User
            {
                Name = model.Name,
                Email = model.email,
                Slug = model.email.Replace("@", "-").Replace(".", "-"),
                PasswordHash = PasswordHasher.Hash(passwordGuid)
            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            emailService.Send(user.Name, user.Email, "Teste envio email", $"Sua senha de usuário é {passwordGuid}");

            return Created("$account/{user.id}", user);
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("Internal server error."));
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromServices] TokenService tokenService, [FromServices] BlogDataContext context, [FromBody] LoginViewModel model)
    {   
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<LoginViewModel>(ModelState.GetErrors()));

        var register = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (register == null)
            return NotFound(new ResultViewModel<dynamic>("Register not found."));

        if(!PasswordHasher.Verify(register.PasswordHash, model.Senha))
            return BadRequest(new ResultViewModel<dynamic>("Password incorrect."));

        try
        {
            var token = tokenService.GenerateToken(register);

            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("Internal server error"));
        }
    }

    [HttpPost("account/upload-image")]
    public async Task<IActionResult> UpdloadImage([FromServices] BlogDataContext context, [FromBody] UploadImageViewModel model)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<UploadImageViewModel>(ModelState.GetErrors()));

            var fileName = $"{Guid.NewGuid().ToString()}.jpg";
            var data = new Regex(@"data:image\/[a-z]+;base64").Replace(model.Base64Image, replacement: "");

            var bytes = Convert.FromBase64String(data);

            await System.IO.File.WriteAllBytesAsync($"wwwroot/images/{fileName}", bytes);

            var user = await context 
                .Users
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == User.Identity.Name);

            if (user == null)
                return NotFound(new ResultViewModel<dynamic>("User not found."));

            user.Image = $"http://localhost:0000/images/{fileName}";
            context.Users.Update(user);
            await context.SaveChangesAsync();
            
            return Ok(new ResultViewModel<string>("Image upload success."));
        }
        catch (Exception)
        {
            return StatusCode(500, new ResultViewModel<string>("Internal server error."));
        }
    }
}