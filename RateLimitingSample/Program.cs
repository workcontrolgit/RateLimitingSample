using Microsoft.EntityFrameworkCore;
using RateLimitingSample;
using RateLimitingSample.Data;
using RateLimitingSample.Enums;
using RateLimitingSample.Extentions;
using RateLimitingSample.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITodoService, TodoService>();
builder.Services.AddSingleton<IEmailService, EmailService>();


builder.Services.AddRateLimiterExtension(builder.Configuration);
builder.Services.AddDbContext<TodoGroupDbContext>(options =>
{
    var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    options.UseSqlite($"Data Source={Path.Join(path, "RateLimitingSample.db")}");
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//using var scope = app.Services.CreateScope();
//var db = scope.ServiceProvider.GetService<TodoGroupDbContext>();
//db?.Database.MigrateAsync();

app.UseHttpsRedirection();

app.UseRateLimiter();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

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
}).DisableRateLimiting();



static string GetTicks() => (DateTime.Now.Ticks & 0x11111).ToString("00000");

app.MapGet("/fixed", () => Results.Ok($"Fixed Window Limiter {GetTicks()}"))
                           .RequireRateLimiting(Policy.FixedWindow.ToString());

app.MapGet("/sliding", () => Results.Ok($"Sliding Window Limiter {GetTicks()}"))
                           .RequireRateLimiting(Policy.SlidingWindow.ToString());

app.MapGet("/token", () => Results.Ok($"Token Limiter {GetTicks()}"))
                           .RequireRateLimiting(Policy.BucketToken.ToString());

app.MapGet("/global", () => Results.Ok($"Global Limiter {GetTicks()}"));

// todoV1 endpoints
app.MapGroup("/todos/v1")
    .MapTodosApiV1()
    .WithTags("Todo Endpoints");

// todoV2 endpoints
app.MapGroup("/todos/v2")
    .MapTodosApiV2()
    .WithTags("Todo Endpoints");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
