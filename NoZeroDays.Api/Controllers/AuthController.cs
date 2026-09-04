using Microsoft.EntityFrameworkCore.Storage;

namespace NoZeroDays.Api.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
public class AuthController(
    UserManager<IdentityUser> userManager,
    ApplicationDbContext applicationDbContext,
    ApplicationIdentityDbContext identityDbContext
) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        IExecutionStrategy strategy = identityDbContext.Database.CreateExecutionStrategy();

        IActionResult? result = null;

        await strategy.ExecuteAsync(async () =>
        {
            using IDbContextTransaction transaction = await identityDbContext.Database.BeginTransactionAsync();
            applicationDbContext.Database.SetDbConnection(identityDbContext.Database.GetDbConnection());
            await applicationDbContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

            var identityUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Name,
            };

            IdentityResult identityResult = await userManager.CreateAsync(identityUser, request.Password);

            if (!identityResult.Succeeded)
            {
                await transaction.RollbackAsync();
                var extensions = new Dictionary<string, object>
                {
                    { "error", identityResult.Errors.ToDictionary(x => x.Code, x => x.Description) }
                };
                result = Problem(detail: "Registration failed",
                    statusCode: StatusCodes.Status400BadRequest,
                    extensions: extensions);
                return;
            }

            var user = request.ToUser();
            user.IdentityId = identityUser.Id;
            await applicationDbContext.Users.AddAsync(user);
            await applicationDbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            result = Ok(user.Id);
        });

        return result!;
    }}
