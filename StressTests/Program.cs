using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;


using var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://localhost:7081");

var callDateNowStep = Step.Create("call todos", async (context) =>
{
    var response = await httpClient.GetAsync("todos/v2");
    if (response.IsSuccessStatusCode)
        return Response.Ok(statusCode: (int)response.StatusCode);
    else
        return Response.Fail(statusCode: (int)response.StatusCode);
});

var scenario = ScenarioBuilder.CreateScenario("Call todos Api", callDateNowStep)
    .WithWarmUpDuration(TimeSpan.FromSeconds(10))
    .WithLoadSimulations(
        LoadSimulation.NewInjectPerSec(_rate: 100, _during: TimeSpan.FromMinutes(1))
    );

NBomberRunner
    .RegisterScenarios(scenario)
    .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
    .Run();

Console.WriteLine("Press any key ...");
Console.ReadKey();