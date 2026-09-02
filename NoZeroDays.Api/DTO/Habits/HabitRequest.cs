using NoZeroDays.Api.Enums;

namespace NoZeroDays.Api.DTO.Habits;

public sealed record HabitRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType Type { get; init; }
    public required FrequencyResponse Frequency { get; init; }
    public required TargetResponse Target { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneRequest? Milestone { get; init; }
}
