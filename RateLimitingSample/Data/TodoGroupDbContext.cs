
using AutoBogus;
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
        var todoFaker = AutoFaker.Generate<Todo>(10);
        modelBuilder.Entity<Todo>().HasData(todoFaker);
    }

}
