using LoadTests.Enums;
using Microsoft.Extensions.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;

namespace LoadTests;

public class ScenarioHelper
{
    public static Scenario GetDisabledLimiterScenario()
    {
        var httpFactory = HttpClientFactory.Create("0");

        var step = Step.Create("weatherforecast", clientFactory: httpFactory, execute: async (context) =>
        {
            try
            {
                var request =
                    Http.CreateRequest("GET", GetBaseUrl() + "/weatherforecast")
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
                //context.Logger.Error(ex.Message);
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario(BombScenario.DisabledScenario.ToString(), step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }


    public static Scenario GetGlobalLimiterScenario()
    {
        var httpFactory = HttpClientFactory.Create("1");

        var step = Step.Create("todos/v1", clientFactory: httpFactory, execute: async (context) =>
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
                //context.Logger.Error(ex.Message);
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario(BombScenario.GlobalScenario.ToString(), step)
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
                //context.Logger.Error(ex.Message);
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario(BombScenario.ConcurrencyScenario.ToString(), step)
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
                //context.Logger.Error(ex.Message);
                return Response.Fail();
            }
        });

        var scenario = ScenarioBuilder.CreateScenario(BombScenario.FixedWindowScenario.ToString(), step)
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
                //context.Logger.Error(ex.Message);
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario(BombScenario.SlidingWindowScenario.ToString(), step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }
    public static Scenario GetUserBasedScenario()
    {
        var httpFactory = HttpClientFactory.Create("5");

        var step = Step.Create("todos/v2/completed", clientFactory: httpFactory, execute: async (context) =>
        {
            try
            {
                var request =
                    Http.CreateRequest("GET", GetBaseUrl() + "/todos/v2/completed")
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
                //context.Logger.Error(ex.Message);
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario(BombScenario.UserBasedScenario.ToString(), step)
                .WithWarmUpDuration(TimeSpan.FromSeconds(5))
                .WithLoadSimulations(
                    LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(2))
                );

        return scenario;
    }
    public static Scenario GetBucketTokenScenario()
    {
        var httpFactory = HttpClientFactory.Create("6");

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
                //context.Logger.Error(ex.Message);
                return Response.Fail();
            }

        });

        var scenario = ScenarioBuilder.CreateScenario(BombScenario.TokenBucketScenario.ToString(), step)
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
