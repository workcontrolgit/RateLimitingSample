using NBomber.Contracts.Stats;
using NBomber.CSharp;




var client = ScenarioHelper.CreateClient();
var scenario = ScenarioHelper.GetToDoTestScenario(client);

NBomberRunner
    .RegisterScenarios(scenario)
    .WithReportFileName("rate_limiting_report")
    .WithReportFolder("rate_limiting_reports")
    .WithReportFormats(ReportFormat.Txt, ReportFormat.Csv, ReportFormat.Html, ReportFormat.Md)
    .Run();

Console.WriteLine("Press any key ...");
Console.ReadKey();