namespace NoZeroDays.Api.Helper;

public static class CookieOptionsExtensions
{
    public static CookieOptions RefreshToken(
        DateTimeOffset expiresAt)
    {
        return new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Lax,
            Expires = expiresAt
        };
    }
}
