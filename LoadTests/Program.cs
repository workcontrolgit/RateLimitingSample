using NBomber.Contracts.Stats;
using NBomber.CSharp;



var scenarioDisableRateLimiting = ScenarioHelper.GetGlobalLimiterScenario();
var scenarioConcurrencyScenario = ScenarioHelper.GetConcurrencyScenario();
var scenarioFixWindowsScenario = ScenarioHelper.GetFixedWindowScenario();
var scenarioSlidingWindowScenario = ScenarioHelper.GetSlidingWindowScenario();
var scenarioBucketTokenScenario = ScenarioHelper.GetBucketTokenScenario();

NBomberRunner
    .RegisterScenarios(scenarioDisableRateLimiting, scenarioConcurrencyScenario, scenarioFixWindowsScenario, scenarioSlidingWindowScenario, scenarioBucketTokenScenario)
    .WithTestSuite("Rate Limiting Test Suite")
    .WithTestName("todo_api_test")
    .WithReportFileName("rate_limiting_report")
    .WithReportFolder("rate_limiting_reports")
    .WithReportFormats(ReportFormat.Txt, ReportFormat.Csv, ReportFormat.Html, ReportFormat.Md)
    .Run();

Console.WriteLine("Press any key ...");
Console.ReadKey();