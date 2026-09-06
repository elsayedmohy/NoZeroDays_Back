namespace NoZeroDays.Api.Mapping.ManualMappings;

public static class TagMapping
{
    
    public static Tag ToEntity(this TagRequest request, string userId)
    {
        return new Tag
        {
            Id = $"t_{Guid.CreateVersion7()}",
            UserId = userId,
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow
        };
    }
    
    public static TagResponse ToResponse(this Tag tag)
    {
        return new TagResponse
        {
            Id = tag.Id,
            Name = tag.Name,
            Description = tag.Description,
            CreatedAt = tag.CreatedAt,
            UpdatedAt = tag.UpdatedAt
        };
    }
    public static void UpdateTag(this Tag tag,  TagRequest request)
    {
        tag.Name = request.Name;
        tag.Description = request.Description;
        tag.UpdatedAt = DateTime.Now;
    }
}
