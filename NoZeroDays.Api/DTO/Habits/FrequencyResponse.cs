namespace NoZeroDays.Api.DTO.Habits;

public sealed record FrequencyResponse
{
    public required FrequencyType Type { get; init; }
    public required int TimesPerPeriod { get; init; }
}
