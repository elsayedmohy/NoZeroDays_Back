namespace NoZeroDays.Api.Database.Configurations;

public sealed class UserConfiguration :IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasMaxLength(500);
        builder.Property(x => x.IdentityId).HasMaxLength(500);
        builder.Property(x => x.Name).HasMaxLength(100);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.IdentityId).IsUnique();
    }
}
