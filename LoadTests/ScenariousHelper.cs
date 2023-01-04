using Microsoft.Extensions.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;


    public static class ScenariousHelper
    {
        public static Scenario GetToDoTestScenario(HttpClient httpClient)
        {
            var stepGetTodosV1 = Step.Create("call todos/v1", async (context) =>
            {
                try
                {
                    var response = await httpClient.GetAsync("todos/v1");
                    return response.IsSuccessStatusCode
                        ? Response.Ok()
                        : Response.Fail();

                }
                catch (Exception ex)
                {
                    return Response.Fail();
                }

            });

        var stepGetTodosV2 = Step.Create("call todos/v2", async context =>
        {
            try
            {
                var response = await httpClient.GetAsync("todos/v2");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }
        });

        var scenario = ScenarioBuilder.CreateScenario("call todos", stepGetTodosV1, stepGetTodosV2)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromSeconds(30))
                );

            return scenario;
        }

    public static HttpClient CreateClient()
    {

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var baseUrl = configuration.GetValue<string>("BaseUrl");
        if (string.IsNullOrEmpty(baseUrl))
            throw new ArgumentException("Cannot read configuration");

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl),
            Timeout = TimeSpan.FromMilliseconds(1750)
        };
        return httpClient;
    }
}
