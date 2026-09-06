namespace NoZeroDays.Api.Mapping.ManualMappings;

public static class HabitMapping
{
    
    public static HabitResponse ToDto(this Habit habit)
    {
        return new HabitResponse
        {
            Id = habit.Id,
            Name = habit.Name,
            Description = habit.Description,
            Type = habit.Type,
            Frequency = new FrequencyResponse
            {
                Type = habit.Frequency.Type,
                TimesPerPeriod = habit.Frequency.TimesPerPeriod
            },
            Target = new TargetResponse
            {
                Value = habit.Target.Value,
                Unit = habit.Target.Unit
            },
            Status = habit.Status,
            Tags = habit.Tags?.Select(tag => new TagResponse
            {
                Id = tag.Id,
                Name = tag.Name,
                Description = tag.Description,
                CreatedAt = tag.CreatedAt,
                UpdatedAt = tag.UpdatedAt
            }).ToList() ?? [],
            IsArchived = habit.IsArchived,
            EndDate = habit.EndDate,
            Milestone = habit.Milestone is null
                ? null
                : new MilestoneResponse
                {
                    Target = habit.Milestone.Target,
                    Current = habit.Milestone.Current
                },
            CreatedAt = habit.CreatedAt,
            UpdatedAt = habit.UpdatedAt,
            LastCompletedAt = habit.LastCompletedAt
        };
    }
    
    public static Habit ToEntity(this HabitRequest request , string userId)
    {
        return new Habit
        {
            Name = request.Name,
            UserId = userId,
            Description = request.Description,
            Type = request.Type,
            Frequency = new Frequency
            {
                Type = request.Frequency.Type,
                TimesPerPeriod = request.Frequency.TimesPerPeriod
            },

            Target = new Target
            {
                Value = request.Target.Value,
                Unit = request.Target.Unit
            },

            EndDate = request.EndDate,

            Milestone = request.Milestone is null
                ? null
                : new Milestone
                {
                    Target = request.Milestone.Target,
                    Current = 0
                }
        };
    }    
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
