namespace NoZeroDays.Api.DTO.Auth;

public sealed record LoginRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}
