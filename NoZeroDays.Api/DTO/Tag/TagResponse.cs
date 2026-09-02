namespace NoZeroDays.Api.DTO.Tag;

public record TagResponse
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; } 
    public DateTime CreatedAt  { get; init; }
    public DateTime? UpdatedAt  { get; init; }
};
