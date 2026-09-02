using Microsoft.EntityFrameworkCore;
using NoZeroDays.Api.Database;

namespace NoZeroDays.Api.Extensions;

public static class DatabaseExtension
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();
        await using ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        try
        {
            await context.Database.MigrateAsync();
            app.Logger.LogInformation("Migrated database successfully.");
        }
        catch (Exception e)
        {
            app.Logger.LogError(e, "An error occurred while migrating the database.");
            Console.WriteLine(e);
            throw;
        }
    }
}
