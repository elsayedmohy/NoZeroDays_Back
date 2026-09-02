namespace NoZeroDays.Api.DTO.Habits;

public sealed record MilestoneRequest
{
    public required int Target { get; init; }
}
