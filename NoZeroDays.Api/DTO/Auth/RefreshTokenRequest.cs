namespace NoZeroDays.Api.DTO.Auth;

public sealed record RefreshTokenRequest
{
    public  string RefreshToken { get; init; }
}
