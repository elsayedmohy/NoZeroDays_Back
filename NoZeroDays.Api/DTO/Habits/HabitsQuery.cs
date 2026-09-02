using Microsoft.AspNetCore.Mvc;
using NoZeroDays.Api.DTO.Common;
using NoZeroDays.Api.Enums;

namespace NoZeroDays.Api.DTO.Habits;

public sealed record HabitsQuery : RequestQuery
{
    public HabitType? Type { get; init; }
    public HabitStatus? Status { get; init; }
}
