using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using NoZeroDays.Api.Helper;
using NoZeroDays.Api.Service.Auth;
using LoginRequest = NoZeroDays.Api.DTO.Auth.LoginRequest;
using RegisterRequest = NoZeroDays.Api.DTO.Auth.RegisterRequest;

namespace NoZeroDays.Api.Controllers;

[ApiController]
[Route("auth")]
[AllowAnonymous]
public class AuthController(
    UserManager<IdentityUser> userManager,
    ApplicationDbContext applicationDbContext,
    ApplicationIdentityDbContext identityDbContext,
    JwtTokenProvider jwtTokenProvider,
    IOptions<JwtOptions> options) : ControllerBase
{
    
   private readonly JwtOptions jwtOptions =  options.Value;
    
    [HttpPost("register")]
    public async Task<ActionResult<AccessTokenResponse>> Register(RegisterRequest request)
    {
        IExecutionStrategy strategy = identityDbContext.Database.CreateExecutionStrategy();

        ActionResult<AccessTokenResponse>? result = null;

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
            var tokenRequest = new TokenRequest(identityUser.Id, identityUser.Email);
            AccessTokenResponse accessTokens = jwtTokenProvider.GenerateToken(tokenRequest);
            var refreshToken = new RefreshToken
            {
                Id = Guid.CreateVersion7(),
                Token = accessTokens.refreshToken,
                UserId = identityUser.Id,
                ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpirationDays),
            };
            
            identityDbContext.RefreshTokens.Add(refreshToken);
            await identityDbContext.SaveChangesAsync();
            await transaction.CommitAsync();

            result = Ok(accessTokens);
        });

        return result!;
    }


    [HttpPost("login")]
    public async Task<ActionResult<AccessTokenResponse>> Login(LoginRequest request)
    {
      IdentityUser? identityUser = await  userManager.FindByEmailAsync(request.Email);
      if (identityUser is null || !await userManager.CheckPasswordAsync(identityUser , request.Password))
      {
          return Unauthorized(); 
      }
      var tokenRequest = new TokenRequest(identityUser.Id, identityUser.Email);
      AccessTokenResponse accessTokens = jwtTokenProvider.GenerateToken(tokenRequest);
      var refreshToken = new RefreshToken
      {
          Id = Guid.CreateVersion7(),
          Token = accessTokens.refreshToken,
          UserId = identityUser.Id,
          ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpirationDays),
      };
            
      identityDbContext.RefreshTokens.Add(refreshToken);
      await identityDbContext.SaveChangesAsync();
      return Ok(accessTokens);
    }


    [HttpPost("refresh")]
    public async Task<ActionResult<AccessTokenResponse>> Refresh(RefreshTokenRequest request)
    {
        RefreshToken? refreshToken = await identityDbContext.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rf => rf.Token == request.RefreshToken);
        if (refreshToken is null || refreshToken.ExpiresAt < DateTime.UtcNow)
        {
            return Unauthorized();
        }
        
        var tokenRequest = new TokenRequest(refreshToken.User.Id, refreshToken.User.Email);
        AccessTokenResponse accessTokens = jwtTokenProvider.GenerateToken(tokenRequest);
        refreshToken.Token  = accessTokens.refreshToken;
        refreshToken.ExpiresAt = DateTime.UtcNow.AddDays(jwtOptions.RefreshTokenExpirationDays);
        await identityDbContext.SaveChangesAsync();
        return Ok(accessTokens);
    }
    
    
}
