namespace RateLimitingSample.Models;
public class RateLimitingSettings
{
    public const string UserBasedTokenBucket = "RateLimitUserBasedTokenBucket";
    public const string UserBasedSlidingWindow = "RateLimitUserBasedSlidingWindow";
    public const string RateLimitGlobalFixedWindow = "RateLimitGlobalFixedWindow";
    public const string TokenBucket = "RateLimitTokenBucket";
    public const string Concurrency = "RateLimitConcurrency";
    public const string SlidingWindow = "RateLimitSlidingWindow";
    public const string FixedWindow = "RateLimitFixedWindow";
    public int PermitLimit { get; set; } = 100;
    public int Window { get; set; } = 10;
    public int ReplenishmentPeriod { get; set; } = 2;
    public int QueueLimit { get; set; } = 2;
    public int SegmentsPerWindow { get; set; } = 8;
    public int TokenLimit { get; set; } = 10;
    public int TokensPerPeriod { get; set; } = 4;
    public bool AutoReplenishment { get; set; } = false;
}
