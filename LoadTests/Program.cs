using NBomber.Contracts.Stats;
using NBomber.CSharp;




var client = ScenariousHelper.CreateClient();
var scenario = ScenariousHelper.GetToDoTestScenario(client);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithReportFormats(ReportFormat.Txt, ReportFormat.Csv, ReportFormat.Html, ReportFormat.Md)
    .Run();

Console.WriteLine("Press any key ...");
Console.ReadKey();