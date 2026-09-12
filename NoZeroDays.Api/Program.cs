WebApplicationBuilder builder = WebApplication.CreateBuilder(args);


builder.AddControllers()
    .AddErrorHandling()
    .AddDatabase()
    .AddObservability()
    .AddApplicationServices()
    .AddAuthenticationServices()
    .AddCorsPolicy();


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrationsAsync();
    await app.SeedInitialDataAsync();
}

app.UseExceptionHandler();
app.UseCors(CorsOptions.PolicyName);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
