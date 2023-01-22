using Microsoft.EntityFrameworkCore;
using RateLimitingSample.Data;
using RateLimitingSample.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace RateLimitingSample;
public static class TodoEndpointsV1
{
    public static RouteGroupBuilder MapTodosApiV1(this RouteGroupBuilder group)
    {
        // Map to NBomber GlobalScenario
        group.MapGet("/", GetAllTodos)
            .WithMetadata(new SwaggerOperationAttribute(summary: "Summary", description: "Descritption Test"));

        // Map to NBomber ConcurrencyScenario
        group.MapGet("/{id}", GetTodo).RequireRateLimiting(Policy.ConcurrencyPolicy.ToString());
        group.MapPost("/", CreateTodo)
            .AddEndpointFilter(async (efiContext, next) =>
            {
                var param = efiContext.GetArgument<TodoDto>(0);

                var validationErrors = Utilities.IsValid(param);

                if (validationErrors.Any())
                {
                    return Results.ValidationProblem(validationErrors);
                }

                return await next(efiContext);
            });

        group.MapPut("/{id}", UpdateTodo);
        group.MapDelete("/{id}", DeleteTodo);

        return group;
    }

    // get all
    public static async Task<IResult> GetAllTodos(ApplicationDbContext database)
    {
        var todos = await database.Todos.AsNoTracking().ToListAsync();
        return TypedResults.Ok(todos);

    }

    // get by id
    public static async Task<IResult> GetTodo(int id, ApplicationDbContext database)
    {
        var todo = await database.Todos.FindAsync(id);

        if (todo != null)
        {
            return TypedResults.Ok(todo);
        }

        return TypedResults.NotFound();
    }

    // create
    public static async Task<IResult> CreateTodo(TodoDto todo, ApplicationDbContext database)
    {
        var newTodo = new Todo
        {
            Title = todo.Title,
            Description = todo.Description,
            IsDone = todo.IsDone
        };

        await database.Todos.AddAsync(newTodo);
        await database.SaveChangesAsync();

        return TypedResults.Created($"/todos/v1/{newTodo.Id}", newTodo);
    }

    // update
    public static async Task<IResult> UpdateTodo(Todo todo, ApplicationDbContext database)
    {
        var existingTodo = await database.Todos.FindAsync(todo.Id);

        if (existingTodo != null)
        {
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsDone = todo.IsDone;

            await database.SaveChangesAsync();

            return TypedResults.Created($"/todos/v1/{existingTodo.Id}", existingTodo);
        }

        return TypedResults.NotFound();
    }

    // delete
    public static async Task<IResult> DeleteTodo(int id, ApplicationDbContext database)
    {
        var todo = await database.Todos.FindAsync(id);

        if (todo != null)
        {
            database.Todos.Remove(todo);
            await database.SaveChangesAsync();

            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}
