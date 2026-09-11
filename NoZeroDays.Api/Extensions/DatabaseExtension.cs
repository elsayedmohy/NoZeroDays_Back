namespace NoZeroDays.Api.Extensions;

public static class DatabaseExtension
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using ApplicationDbContext applicationDbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();
        await using ApplicationDbContext identityDbContext = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();
        try
        {
            await applicationDbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Migrated Application database successfully.");
            await identityDbContext.Database.MigrateAsync();
            app.Logger.LogInformation("Migrated Identity database successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while migrating the database.");
            Console.WriteLine(e);
            throw;
        }
    }


    public static async Task SeedInitialDataAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        RoleManager<IdentityRole> roleManager = scope.ServiceProvider.
            GetRequiredService<RoleManager<IdentityRole>>();
        try
        {
            if (!await roleManager.RoleExistsAsync(Roles.Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
            }

            if (!await roleManager.RoleExistsAsync(Roles.User))
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.User));
            }
            app.Logger.LogInformation("Created Role successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "Failed to create role due to an exception.");
            throw;
        }
    }
}
