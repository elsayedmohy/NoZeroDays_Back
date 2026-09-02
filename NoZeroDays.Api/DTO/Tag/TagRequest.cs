using System.ComponentModel.DataAnnotations;

namespace NoZeroDays.Api.DTO.Tag;

public record TagRequest
{
    public string Name { get; init; }
    public string? Description { get; init; }
}
