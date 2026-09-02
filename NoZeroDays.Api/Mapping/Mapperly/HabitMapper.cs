using NoZeroDays.Api.DTO.Habits;
using NoZeroDays.Api.Entities;
using Riok.Mapperly.Abstractions;

namespace NoZeroDays.Api.Mapping.Mapperly;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public partial class HabitMapper
{
    [MapperIgnoreSource(nameof(Habit.HabitTags))]
    public partial HabitResponse ToDto(Habit habit);
    
    public partial Habit ToEntity(HabitRequest habit);
}
