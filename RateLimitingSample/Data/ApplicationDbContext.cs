
using AutoBogus;
using Microsoft.EntityFrameworkCore;

namespace RateLimitingSample.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

        var todos = todoFaker.Generate(10000);
        modelBuilder.Entity<Todo>().HasData(todos);
    }

}
