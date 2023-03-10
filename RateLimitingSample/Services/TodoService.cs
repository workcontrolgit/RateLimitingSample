using Microsoft.EntityFrameworkCore;
using RateLimitingSample.Data;

namespace RateLimitingSample.Services;

public class TodoService : ITodoService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IEmailService _emailService;

    public TodoService(ApplicationDbContext dbContext, IEmailService emailService)
    {
        _dbContext = dbContext;
        _emailService = emailService;
    }

    public async ValueTask<Todo?> Find(int id)
    {
        return await _dbContext.Todos.FindAsync(id);
    }

    public async Task<List<Todo>> GetAll()
    {
        return await _dbContext.Todos.AsNoTracking().ToListAsync();
    }

    public async Task Add(Todo todo)
    {
        await _dbContext.Todos.AddAsync(todo);

        if (await _dbContext.SaveChangesAsync() > 0)
            await _emailService.Send("hello@microsoft.com", $"New todo has been added: {todo.Title}");
    }

    public async Task Update(Todo todo)
    {
        _dbContext.Todos.Update(todo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task Remove(Todo todo)
    {
        _dbContext.Todos.Remove(todo);
        await _dbContext.SaveChangesAsync();
    }

    public Task<List<Todo>> GetIncompleteTodos()
    {
        return _dbContext.Todos.Where(t => t.IsDone == false).AsNoTracking().ToListAsync();
    }

    public Task<List<Todo>> GetCompleteTodos()
    {
        return _dbContext.Todos.Where(t => t.IsDone == true).AsNoTracking().ToListAsync();
    }

}
