using RateLimitingSample.Data;

namespace RateLimitingSample.Services;

public interface ITodoService
{
    Task<List<Todo>> GetAll();

    Task<List<Todo>> GetIncompleteTodos();
    Task<List<Todo>> GetCompleteTodos();

    ValueTask<Todo?> Find(int id);

    Task Add(Todo todo);

    Task Update(Todo todo);

    Task Remove(Todo todo);
}
