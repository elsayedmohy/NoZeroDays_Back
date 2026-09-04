namespace NoZeroDays.Api.Mapping.ManualMappings;

public static class UserMapping
{
    public static User ToUser(this RegisterRequest register)
    {
        return new User
        {
            Id = $"u_{Guid.CreateVersion7()}",
            Name = register.Name,
            Email = register.Email,
            CreatedAt = DateTime.UtcNow,
        };
    }
}
