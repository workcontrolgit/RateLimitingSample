using IntegrationTests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using RateLimitingSample.Data;
using RateLimitingSample.Services;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests;

[Collection("Sequential")]
public class TodoEndpointsV2Tests : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory;
    private readonly HttpClient _httpClient;

    public TodoEndpointsV2Tests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _httpClient = factory.CreateClient();
    }

    public static IEnumerable<object[]> InvalidTodos => new List<object[]>
    {
        new object[] { new TodoDto { Title = "", Description = "Test description", IsDone = false }, "Name is empty" },
        new object[] { new TodoDto { Title = "no", Description = "Test description", IsDone = false }, "Name length < 3" }
    };

    [Theory]
    [MemberData(nameof(InvalidTodos))]
    public async Task PostTodoWithValidationProblems(TodoDto todo, string errorMessage)
    {
        var response = await _httpClient.PostAsJsonAsync("/todos/v2", todo);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        var problemResult = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();

        Assert.NotNull(problemResult?.Errors);
        Assert.Collection(problemResult.Errors, (error) => Assert.Equal(errorMessage, error.Value.First()));
    }

    [Fact]
    public async Task PostTodoWithValidParameters()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
            if (db != null && db.Todos.Any())
            {
                db.Todos.RemoveRange(db.Todos);
                await db.SaveChangesAsync();
            }
        }

        var client = _factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IEmailService, TestEmailService>();
            });
        }).CreateClient();

        var response = await client.PostAsJsonAsync("/todos/v2", new TodoDto
        {
            Title = "Test title",
            Description = "Test description",
            IsDone = false
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var todos = await client.GetFromJsonAsync<List<Todo>>("/todos/v2");

        Assert.NotNull(todos);
        Assert.Single(todos);

        Assert.Collection(todos, (todo) =>
        {
            Assert.Equal("Test title", todo.Title);
            Assert.Equal("Test description", todo.Description);
            Assert.False(todo.IsDone);
        });
    }
}
