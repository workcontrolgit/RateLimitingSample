using LoadTests;
using NBomber.Contracts.Stats;
using NBomber.CSharp;

var scenarioDisabledRateLimiting = ScenarioHelper.GetDisabledLimiterScenario();
var scenarioGlobalRateLimiting = ScenarioHelper.GetGlobalLimiterScenario();
var scenarioConcurrencyScenario = ScenarioHelper.GetConcurrencyScenario();
var scenarioFixedWindowScenario = ScenarioHelper.GetFixedWindowScenario();
var scenarioSlidingWindowScenario = ScenarioHelper.GetSlidingWindowScenario();
var scenarioUserBasedRateLimitScenario = ScenarioHelper.GetUserBasedScenario();
var scenarioBucketTokenScenario = ScenarioHelper.GetBucketTokenScenario();

// configuring NBomber tests via configuration files
// https://nbomber.com/docs/json-config

//NBomberRunner
//    .RegisterScenarios(scenarioDisabledRateLimiting, scenarioGlobalRateLimiting, scenarioConcurrencyScenario, scenarioFixedWindowScenario, scenarioSlidingWindowScenario, scenarioUserBasedRateLimitScenario, scenarioBucketTokenScenario)
//    .LoadConfig("config.json")
//    .Run();

// run NBomber using builder

NBomberRunner
    .RegisterScenarios(scenarioDisabledRateLimiting, scenarioGlobalRateLimiting, scenarioConcurrencyScenario, scenarioFixedWindowScenario, scenarioSlidingWindowScenario, scenarioUserBasedRateLimitScenario, scenarioBucketTokenScenario)
    //.RegisterScenarios(scenarioGlobalRateLimiting)
    .WithTestSuite("Rate Limiting Test Suite")
    .WithTestName("todo_api_test")
    .WithReportFileName("my_reports")
    .WithReportFolder("./my_reports")
    .WithReportFormats(ReportFormat.Html, ReportFormat.Md, ReportFormat.Txt, ReportFormat.Csv)
    .Run();


Console.WriteLine("Press any key ...");
Console.ReadKey();