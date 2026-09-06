namespace NoZeroDays.Api.Database.Configurations;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(64);
        builder.Property(x => x.UserId).HasMaxLength(500);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.HasIndex(x => new {x.UserId, x.Name}).IsUnique();

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId);
    }
}
