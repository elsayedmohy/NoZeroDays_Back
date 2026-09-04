
namespace NoZeroDays.Api.Controllers;


[ApiController]
[Route("users")]
public sealed class UsersController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult> GetUserById(string id)
    {
        UserResponse? user = await dbContext.Users.Where(usr => usr.Id == id)
            .Select(UserProjections.ToResponse)
            .FirstOrDefaultAsync();
        if (user is null)
        {
            return NotFound(); 
        }

        return Ok(ApiResponse<UserResponse>.Ok(user));
    }
}
