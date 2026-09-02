using NoZeroDays.Api.DTO.Tag;
using NoZeroDays.Api.Enums;

namespace NoZeroDays.Api.DTO.Habits;

public sealed record HabitResponse
{

    public required string Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required HabitType Type { get; init; }
    public required FrequencyResponse Frequency { get; init; }
    public required TargetResponse Target { get; init; }
    public required HabitStatus Status { get; init; }
    public  List<TagResponse> Tags { get; init; }
    public required bool IsArchived { get; init; }
    public DateOnly? EndDate { get; init; }
    public MilestoneResponse? Milestone { get; init; }
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? LastCompletedAt { get; init; }
}
