using Microsoft.Extensions.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;

public static class ScenarioHelper
    {

    public static Scenario GetGlobalLimiterScenario()
    {
        var httpFactory = HttpClientFactory.Create("1");

        var stepDisableRateLimiting = Step.Create("todos/v1", clientFactory: httpFactory, execute: async (context) =>
        {
            try
            {
                var request =
                    Http.CreateRequest("GET", GetBaseUrl() + "/todos/v1")
                        .WithHeader("Accept", "text/html")
                        .WithCheck(async (response) =>
                            response.IsSuccessStatusCode
                                ? Response.Ok()
                                : Response.Fail()
                        );

                var response = await Http.Send(request, context);
                return response;

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario("GlobalLimiter", stepDisableRateLimiting)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }


    public static Scenario GetConcurrencyScenario()
    {
        var httpFactory = HttpClientFactory.Create("2");

        var step = Step.Create("todos/v1/1", clientFactory: httpFactory, execute: async (context) =>
        {
            try
            {
                var request =
                    Http.CreateRequest("GET", GetBaseUrl() + "/todos/v1/1")
                        .WithHeader("Accept", "text/html")
                        .WithCheck(async (response) =>
                            response.IsSuccessStatusCode
                                ? Response.Ok()
                                : Response.Fail()
                        );

                var response = await Http.Send(request, context);
                return response;

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario("Concurrency", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }    
    public static Scenario GetFixedWindowScenario()
    {
        var httpFactory = HttpClientFactory.Create("3");

        var step = Step.Create("todos/v2", clientFactory: httpFactory, execute: async (context) =>
        {
            try
            {
                var request =
                    Http.CreateRequest("GET", GetBaseUrl() + "/todos/v2")
                        .WithHeader("Accept", "text/html")
                        .WithCheck(async (response) =>
                            response.IsSuccessStatusCode
                                ? Response.Ok()
                                : Response.Fail()
                        );

                var response = await Http.Send(request, context);
                return response;

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }
        });

        var scenario = ScenarioBuilder.CreateScenario("FixedWindow", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }
    public static Scenario GetSlidingWindowScenario()
    {
        var httpFactory = HttpClientFactory.Create("4");

        var step = Step.Create("todos/v2/incompleted", clientFactory: httpFactory, execute: async (context) =>
        {
            try
            {
                var request =
                    Http.CreateRequest("GET", GetBaseUrl() + "/todos/v2/incompleted")
                        .WithHeader("Accept", "text/html")
                        .WithCheck(async (response) =>
                            response.IsSuccessStatusCode
                                ? Response.Ok()
                                : Response.Fail()
                        );

                var response = await Http.Send(request, context);
                return response;

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario("SlidingWindow", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }    
    public static Scenario GetBucketTokenScenario()
    {
        var httpFactory = HttpClientFactory.Create("5");

        var step = Step.Create("todos/v2/1", clientFactory: httpFactory, execute: async (context) =>
        {
            try
            {
                var request =
                    Http.CreateRequest("GET", GetBaseUrl() + "/todos/v2/1")
                        .WithHeader("Accept", "text/html")
                        .WithCheck(async (response) =>
                            response.IsSuccessStatusCode
                                ? Response.Ok()
                                : Response.Fail()
                        );

                var response = await Http.Send(request, context);
                return response;

            }
            catch (Exception ex)
            {
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario("BucketToken", step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }

    private static string GetBaseUrl()
    {

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var baseUrl = configuration.GetValue<string>("BaseUrl");
        return baseUrl;
    }

}
