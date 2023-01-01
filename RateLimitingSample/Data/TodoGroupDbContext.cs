
using Microsoft.EntityFrameworkCore;

namespace RateLimitingSample.Data;

public class TodoGroupDbContext : DbContext
{
    public TodoGroupDbContext(DbContextOptions<TodoGroupDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
    public DbSet<Todo> Todos => Set<Todo>();

}
