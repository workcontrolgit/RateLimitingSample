using NBomber.CSharp;



var scenarioDisabledRateLimiting = ScenarioHelper.GetDisabledLimiterScenario();
var scenarioGlobalRateLimiting = ScenarioHelper.GetGlobalLimiterScenario();
var scenarioConcurrencyScenario = ScenarioHelper.GetConcurrencyScenario();
var scenarioFixedWindowScenario = ScenarioHelper.GetFixedWindowScenario();
var scenarioSlidingWindowScenario = ScenarioHelper.GetSlidingWindowScenario();
var scenarioUserBasedRateLimitScenario = ScenarioHelper.GetUserBasedScenario();
var scenarioBucketTokenScenario = ScenarioHelper.GetBucketTokenScenario();

NBomberRunner
    .RegisterScenarios(scenarioDisabledRateLimiting, scenarioGlobalRateLimiting, scenarioConcurrencyScenario, scenarioFixedWindowScenario, scenarioSlidingWindowScenario, scenarioUserBasedRateLimitScenario, scenarioBucketTokenScenario)
    .LoadConfig("config.json")
    .Run();


//NBomberRunner
//    .RegisterScenarios(scenarioDisabledRateLimiting, scenarioGlobalRateLimiting, scenarioConcurrencyScenario, scenarioFixedWindowScenario, scenarioSlidingWindowScenario, scenarioUserBasedRateLimitScenario, scenarioBucketTokenScenario)
//    .WithTestSuite("Rate Limiting Test Suite")
//    .WithTestName("todo_api_test")
//    .WithReportFileName("rate_limiting_report")
//    .WithReportFolder("rate_limiting_reports")
//    .WithReportFormats(ReportFormat.Txt, ReportFormat.Csv, ReportFormat.Html, ReportFormat.Md)
//    .Run();


Console.WriteLine("Press any key ...");
Console.ReadKey();