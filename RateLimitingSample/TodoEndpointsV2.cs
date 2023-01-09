using RateLimitingSample.Data;
using RateLimitingSample.Enums;
using RateLimitingSample.Services;

namespace RateLimitingSample;

public static class TodoEndpointsV2
{
    public static RouteGroupBuilder MapTodosApiV2(this RouteGroupBuilder group)
    {
        // Map to NBomber FixedWindowScenario
        group.MapGet("/", GetAllTodos).RequireRateLimiting(Policy.FixedWindowPolicy.ToString());
        // Map to NBomber SlidingWindowScenario
        group.MapGet("/incompleted", GetAllIncompletedTodos).RequireRateLimiting(Policy.SlidingWindowPolicy.ToString());
        // Map to NBomber UserBasedScenario
        group.MapGet("/completed", GetAllCompletedTodos).RequireRateLimiting(Policy.UserBasedPolicy.ToString());
        // Map to NBomber TokenBucketScenario
        group.MapGet("/{id}", GetTodo).RequireRateLimiting(Policy.TokenBucketPolicy.ToString());
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
            })
            .RequireRateLimiting(Policy.TokenBucketPolicy.ToString())
            ;

        group.MapPut("/{id}", UpdateTodo).RequireRateLimiting(Policy.TokenBucketPolicy.ToString());
        group.MapDelete("/{id}", DeleteTodo).RequireRateLimiting(Policy.TokenBucketPolicy.ToString());

        return group;
    }

    // get all
    public static async Task<IResult> GetAllTodos(ITodoService todoService)
    {
        var todos = await todoService.GetAll();
        return TypedResults.Ok(todos);
    }
    // get incompleted
    public static async Task<IResult> GetAllIncompletedTodos(ITodoService todoService)
    {
        var todos = await todoService.GetIncompleteTodos();
        return TypedResults.Ok(todos);
    }
// get completed
    public static async Task<IResult> GetAllCompletedTodos(ITodoService todoService)
    {
        var todos = await todoService.GetCompleteTodos();
        return TypedResults.Ok(todos);
    }


    // get by id
    public static async Task<IResult> GetTodo(int id, ITodoService todoService)
    {
        var todo = await todoService.Find(id);

        if (todo != null)
        {
            return TypedResults.Ok(todo);
        }

        return TypedResults.NotFound();
    }

    // create
    public static async Task<IResult> CreateTodo(TodoDto todo, ITodoService todoService)
    {
        var newTodo = new Todo
        {
            Title = todo.Title,
            Description = todo.Description,
            IsDone = todo.IsDone
        };

        await todoService.Add(newTodo);

        return TypedResults.Created($"/todos/v2/{newTodo.Id}", newTodo);
    }

    // update
    public static async Task<IResult> UpdateTodo(Todo todo, ITodoService todoService)
    {
        var existingTodo = await todoService.Find(todo.Id);

        if (existingTodo != null)
        {
            existingTodo.Title = todo.Title;
            existingTodo.Description = todo.Description;
            existingTodo.IsDone = todo.IsDone;

            await todoService.Update(existingTodo);

            return TypedResults.Created($"/todos/v2/{existingTodo.Id}", existingTodo);
        }

        return TypedResults.NotFound();
    }

    // delete
    public static async Task<IResult> DeleteTodo(int id, ITodoService todoService)
    {
        var todo = await todoService.Find(id);

        if (todo != null)
        {
            await todoService.Remove(todo);
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}
