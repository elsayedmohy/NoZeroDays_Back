namespace NoZeroDays.Api.Mapping.Projections;

public static class TagProjections
{
    public static Expression<Func<Tag, TagResponse>> ToResponse =>
        tag => new TagResponse()
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt,
        };
}
