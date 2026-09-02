namespace NoZeroDays.Api.DTO.Habits;

public sealed record TargetResponse
{
    public required int Value { get; init; }
    public required string Unit { get; init; }
}
