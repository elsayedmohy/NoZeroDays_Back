
namespace NoZeroDays.Api.DTO.Auth;

public sealed record TokenRequest(string userId, string Email , IList<string> Roles);
