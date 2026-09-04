namespace NoZeroDays.Api.Mapping.Projections;

public static class UserProjections
{
    public static Expression<Func<User, UserResponse>> ToResponse =>
        user => new UserResponse()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };

}
