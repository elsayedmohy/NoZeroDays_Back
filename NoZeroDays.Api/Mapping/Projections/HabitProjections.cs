using System.Linq.Expressions;
using NoZeroDays.Api.DTO.Habits;
using NoZeroDays.Api.DTO.Tag;
using NoZeroDays.Api.Entities;

namespace NoZeroDays.Api.Mapping.Projections;

public static class HabitProjections
{
    public static Expression<Func<Habit, HabitResponse>> ToResponse =>
        habit => new HabitResponse
        {
            Id = habit.Id,
            Name = habit.Name,
            Description = habit.Description,
            Type = habit.Type,
            Frequency =  new FrequencyResponse
            {
                Type = habit.Frequency.Type,
                TimesPerPeriod = habit.Frequency.TimesPerPeriod,
            },
            Target = new TargetResponse
            {
                Unit = habit.Target.Unit,
                Value = habit.Target.Value
            } ,
            Status = habit.Status,
            IsArchived = habit.IsArchived,
            EndDate = habit.EndDate,
            Milestone = habit.Milestone == null ? null : new MilestoneResponse
            {
                Target = habit.Milestone.Target,
                Current = habit.Milestone.Current,
            },
            CreatedAt = habit.CreatedAt,
            UpdatedAt = habit.UpdatedAt,
            LastCompletedAt = habit.LastCompletedAt,

            Tags = habit.Tags
                .Select(tag => new TagResponse
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description,
                    CreatedAt = tag.CreatedAt
                })
                .ToList()
        };
}
