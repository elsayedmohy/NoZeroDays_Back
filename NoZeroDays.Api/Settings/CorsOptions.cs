namespace NoZeroDays.Api.Settings;

public sealed class CorsOptions
{
    public const string PolicyName = "NoZeroDaysCorsPolicy";
    public const string SectionName = "Cors";
    public required string[] AllowedOrigins { get; set; }
}
