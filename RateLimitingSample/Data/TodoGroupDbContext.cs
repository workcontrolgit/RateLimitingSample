
using AutoBogus;
using Bogus;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var id = 1;
        var todoFaker = new AutoFaker<Todo>()
        .RuleFor(o => o.Id, f => id++);

        var todos = todoFaker.Generate(100);
        modelBuilder.Entity<Todo>().HasData(todos);
    }

}
