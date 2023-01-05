using Microsoft.Extensions.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;


    public static class ScenariousHelper
    {
        public static Scenario GetToDoTestScenario(HttpClient httpClient)
        {
            var stepDisableRateLimiting = Step.Create("todos/v1, DisableRateLimiting", async (context) =>
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

        var stepFixWindows = Step.Create("todos/v2, FixedWindow", async context =>
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

        var stepSlidingWindow = Step.Create("todos/v2, SlidingWindow", async context =>
        {
            try
            {
                var response = await httpClient.GetAsync("todos/v2/incompleted");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }
        });

        var stepBucketToken = Step.Create("todos/v2, BucketToken", async context =>
        {
            try
            {
                var response = await httpClient.GetAsync("todos/v2/1");

                return response.IsSuccessStatusCode
                    ? Response.Ok()
                    : Response.Fail();

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }
        });


        var scenario = ScenarioBuilder.CreateScenario("Rate Limiting", stepDisableRateLimiting, stepFixWindows, stepSlidingWindow, stepBucketToken)
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
