namespace NoZeroDays.Api.Mapping.ManualMappings;

public static class HabitMapping
{
    public static void HabitToDto(this Habit habit,  HabitRequest request)
    {
        habit.Name = request.Name;
        habit.Description = request.Description;
        habit.EndDate = request.EndDate;
        habit.Type = request.Type;
        habit.Frequency = new Frequency
        {
            Type = request.Frequency.Type,
            TimesPerPeriod =  request.Frequency.TimesPerPeriod
        };
        habit.Target = new Target
        {
            Value = request.Target.Value,
            Unit = request.Target.Unit
        };
        if (request.Milestone != null)
        {
            habit.Milestone ??= new Milestone();
            habit.Milestone.Target =  request.Milestone.Target;
        }
        habit.UpdatedAt = DateTime.Now;
    }
    public static readonly SortMappingDefinition<HabitResponse, Habit> SortMapping = new()
    {
        Mappings =
        [
            new SortMapping(nameof(HabitResponse.Name), nameof(Habit.Name)),
            new SortMapping(nameof(HabitResponse.Description), nameof(Habit.Description)),
            new SortMapping(nameof(HabitResponse.Type), nameof(Habit.Type)),
            new SortMapping(
                $"{nameof(HabitResponse.Frequency)}.{nameof(FrequencyResponse.Type)}",
                $"{nameof(Habit.Frequency)}.{nameof(Frequency.Type)}"),
            new SortMapping(
                $"{nameof(HabitResponse.Frequency)}.{nameof(FrequencyResponse.TimesPerPeriod)}",
                $"{nameof(Habit.Frequency)}.{nameof(Frequency.TimesPerPeriod)}"),
            new SortMapping(
                $"{nameof(HabitResponse.Target)}.{nameof(TargetResponse.Value)}",
                $"{nameof(Habit.Target)}.{nameof(Target.Value)}"),
            new SortMapping(
                $"{nameof(HabitResponse.Target)}.{nameof(TargetResponse.Unit)}",
                $"{nameof(Habit.Target)}.{nameof(Target.Unit)}"),
            new SortMapping(nameof(HabitResponse.Status), nameof(Habit.Status)),
            new SortMapping(nameof(HabitResponse.EndDate), nameof(Habit.EndDate)),
            new SortMapping(nameof(HabitResponse.CreatedAt), nameof(Habit.CreatedAt)),
            new SortMapping(nameof(HabitResponse.UpdatedAt), nameof(Habit.UpdatedAt)),
            new SortMapping(nameof(HabitResponse.LastCompletedAt), nameof(Habit.LastCompletedAt))
        ]
    };
}
