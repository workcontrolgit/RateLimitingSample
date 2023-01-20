using AutoWrapper;
using Microsoft.EntityFrameworkCore;
using RateLimitingSample;
using RateLimitingSample.Data;
using RateLimitingSample.Extentions;
using RateLimitingSample.Services;
using Serilog;


try
{
    var builder = WebApplication.CreateBuilder(args);
    // For Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddTransient<ITodoService, TodoService>();
    builder.Services.AddSingleton<IEmailService, EmailService>();


    builder.Services.AddRateLimiterExtension(builder.Configuration);
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        // check UseMSSQLLDatabase in aspSettings.json
        var useMSSQLLDatabase = builder.Configuration.GetValue<bool>("UseMSSQLLDatabase");

        if (useMSSQLLDatabase)
        {
            // use MSSQL
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        }
        else
        {
            // use UseInMemoryDatabase
            options.UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}");
        }
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // Ensure that the database for the context exists.For quick prototype
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        // For quick prototype.
        dbContext.Database.EnsureCreated();
    }

    app.UseHttpsRedirection();
    app.MapControllers();
    // For AutoWrapper
    app.UseApiResponseAndExceptionWrapper();

    // For Microsoft.AspNetCore.RateLimiting
    app.UseRateLimiter();


    var summaries = new[]
    {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

    // Map to NBomber DisabledScenario
    app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
        return forecast;
    }).DisableRateLimiting()
    .WithTags("Weather");


    // todoV1 endpoints
    app.MapGroup("/todos/v1")
        .MapTodosApiV1()
        .WithTags("Todos");

    // todoV2 endpoints
    app.MapGroup("/todos/v2")
        .MapTodosApiV2()
        .WithTags("Todos");



    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}



internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public partial class Program { }
