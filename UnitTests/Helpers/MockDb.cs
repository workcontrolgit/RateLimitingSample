using Microsoft.EntityFrameworkCore;
using RateLimitingSample.Data;

namespace UnitTests.Helpers;

public class MockDb : IDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new ApplicationDbContext(options);
    }
}
