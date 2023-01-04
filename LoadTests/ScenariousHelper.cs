using Microsoft.Extensions.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;


    public static class ScenariousHelper
    {
        public static Scenario GetToDoTestScenario(HttpClient httpClient)
        {
            var callDateNowStep = Step.Create("call todos/v2", async (context) =>
            {
                var response = await httpClient.GetAsync("todos/v2");
                if (response.IsSuccessStatusCode)
                    return Response.Ok(statusCode: (int)response.StatusCode);
                else
                    return Response.Fail(statusCode: (int)response.StatusCode);
            });

            var scenario = ScenarioBuilder.CreateScenario("call todos/v2", callDateNowStep)
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

        var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(baseUrl);
        return httpClient;
    }
}
