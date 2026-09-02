using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoZeroDays.Api.Entities;

namespace NoZeroDays.Api.Database.Configurations;

public class HabitConfiguration : IEntityTypeConfiguration<Habit>
{
    public void Configure(EntityTypeBuilder<Habit> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(64);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.OwnsOne(habit => habit.Frequency);
        builder.OwnsOne(habit => habit.Target, target =>
            target.Property(t => t.Unit).HasMaxLength(100));
        builder.OwnsOne(habit => habit.Milestone);
        builder.HasMany(habit => habit.Tags).WithMany().UsingEntity<HabitTag>();
    }
}
