namespace NoZeroDays.Api.Database.Configurations;

public class HabitTagConfiguration : IEntityTypeConfiguration<HabitTag>
{
    public void Configure(EntityTypeBuilder<HabitTag> builder)
    {
        builder.HasKey(x => new {x.HabitId , x.TagId});
        builder.HasOne<Tag>().WithMany().HasForeignKey(x => x.TagId);
        builder.HasOne<Habit>().WithMany(h => h.HabitTags).HasForeignKey(x => x.HabitId);
    }
}
