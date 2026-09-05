using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using NoZeroDays.Api.DTO.Auth;
using NoZeroDays.Api.Helper;

namespace NoZeroDays.Api.Service.Auth;

public sealed class JwtTokenProvider(IOptions<JwtOptions> options)
{
    private readonly JwtOptions jwtOptions = options.Value;


    public AccessTokenResponse GenerateToken(TokenRequest tokenRequest)
    {
        return new AccessTokenResponse(
            GenerateAccessToken(tokenRequest),
            GenerateRefreshToken());
    }


    public string GenerateAccessToken(TokenRequest tokenRequest)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims =
        [
            new (JwtRegisteredClaimNames.Sub,tokenRequest.userId),
            new (JwtRegisteredClaimNames.Email,tokenRequest.Email)
        ];
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpirationInMinutes),
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            SigningCredentials =  signingCredentials
        };
        var handler = new JsonWebTokenHandler();
        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }


    public static string GenerateRefreshToken()
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(randomBytes);
    }
}
