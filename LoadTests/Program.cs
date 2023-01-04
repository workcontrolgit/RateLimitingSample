using Microsoft.Extensions.Configuration;
using NBomber.Configuration;
using NBomber.Contracts;
using NBomber.CSharp;


var client = ScenariousHelper.CreateClient();
var scenario = ScenariousHelper.GetToDoTestScenario(client);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithReportFormats(ReportFormat.Html, ReportFormat.Md)
    .Run();

Console.WriteLine("Press any key ...");
Console.ReadKey();