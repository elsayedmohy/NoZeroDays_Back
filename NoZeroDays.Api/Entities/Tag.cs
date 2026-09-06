namespace NoZeroDays.Api.Entities;

public sealed class Tag
{
    public string Id { get; set; }
    
    public string UserId { get; set; }
    public string Name { get; set; } =  string.Empty;
    public string? Description { get; set; } 
    public DateTime CreatedAt  { get; set; }
    public DateTime? UpdatedAt  { get; set; }
}
