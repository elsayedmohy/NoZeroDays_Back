using System.Security.Claims;
using NoZeroDays.Api.Service.Auth;

namespace NoZeroDays.Api.Controllers;

[Authorize]
[ApiController]
[Route("users")]
public sealed class UsersController(ApplicationDbContext dbContext,
    UserContext userContext) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserById(string id)
    {
        
        string? userId = await userContext.GetUserId();
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized();
        }

        if (id != userId)
        {
            return Forbid();
        }
        
        UserResponse? user = await dbContext.Users
            .Where(usr => usr.Id == id)
            .Select(UserProjections.ToResponse)
            .FirstOrDefaultAsync();
        if (user is null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<UserResponse>.Ok(user));
    }


    [HttpGet("me")]
    public async Task<ActionResult> GetCurrentUser()
    {

        string? userId = await userContext.GetUserId();
        
        if (userId is null)
        {
            return Unauthorized();
        }


        UserResponse? user = await dbContext.Users
            .Where(usr => usr.Id == userId)
            .Select(UserProjections.ToResponse)
            .FirstOrDefaultAsync();
        if (user is null)
        {
            return NotFound();
        }

        return Ok(ApiResponse<UserResponse>.Ok(user));
    }
}
