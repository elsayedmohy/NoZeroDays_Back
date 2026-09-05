namespace NoZeroDays.Api.Helper;

public class JwtOptions
{
    public string Issuer  { get; set; }
    public string Audience { get; set; }
    public string Key { get; set; }
    public int ExpirationInMinutes  { get; set; }
    public int RefreshTokenExpirationDays  { get; set; }
}
