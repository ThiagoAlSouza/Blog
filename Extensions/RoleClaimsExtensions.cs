using System.Security.Claims;
using Blog.Models;

namespace Blog.Extensions;

public static class RoleClaimsExtensions
{
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
        };
        
        result.AddRange(
            user.Roles.Select(x => new Claim(ClaimTypes.Role, x.Slug)));

        return result;
    }
}