using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers;

[ApiController]
[Authorize]
[Route("v1")]
public class RoleController : ControllerBase
{

}