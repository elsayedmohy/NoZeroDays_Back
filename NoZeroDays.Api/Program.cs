using NoZeroDays.Api.Middleware;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using NoZeroDays.Api.Database;
using NoZeroDays.Api.DTO.Habits;
using NoZeroDays.Api.Entities;
using NoZeroDays.Api.Extensions;
using NoZeroDays.Api.Mapping.ManualMappings;
using NoZeroDays.Api.Service;
using NoZeroDays.Api.Service.Sorting;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataMapper();

builder.Services.AddControllers(options =>
        options.ReturnHttpNotAcceptable = true)
    .AddNewtonsoftJson()
    .AddXmlSerializerFormatters();

builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = context =>
        context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier)
);
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddOpenApi();


string connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString),
            sqlOptions =>
            {
                sqlOptions.MigrationsHistoryTable(
                    HistoryRepository.DefaultTableName);

                sqlOptions.EnableRetryOnFailure();
            })
        .UseSnakeCaseNamingConvention());

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
    .WithTracing(tracing => tracing
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation())
    .WithMetrics(metrics => metrics
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation())
    .UseOtlpExporter();

builder.Logging.AddOpenTelemetry(options =>
{
    options.IncludeScopes = true;
    options.IncludeFormattedMessage = true;
});


builder.Services.AddTransient<SortMappingProvider>();
builder.Services.AddSingleton<ISortMappingDefinition,
    SortMappingDefinition<HabitResponse, Habit>>(_ => HabitMapping.SortMapping);


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    await app.ApplyMigrationsAsync();
}

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
