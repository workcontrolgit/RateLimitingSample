using Microsoft.AspNetCore.RateLimiting;
using RateLimitingSample.Enums;
using RateLimitingSample.Models;
using System.Globalization;
using System.Threading.RateLimiting;

namespace RateLimitingSample.Extentions
{
    public static class ServiceExtensions
    {
        public static void AddRateLimiterExtension(this IServiceCollection services, IConfiguration configuration)
        {

            var myOptions = new MyRateLimitOptions();
            configuration.GetSection(MyRateLimitOptions.MyRateLimit).Bind(myOptions);

            services.AddRateLimiter(config =>
            {
                config.OnRejected = (context, cancellationToken) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                    return new ValueTask();
                };

                config.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = myOptions.PermitLimit,
                            Window = TimeSpan.FromSeconds(myOptions.Window),
                            QueueLimit = myOptions.QueueLimit,
                            AutoReplenishment = myOptions.AutoReplenishment
                        }));

                config.AddConcurrencyLimiter(policyName: Policy.Concurrency.ToString(), options =>
                {
                    options.PermitLimit = myOptions.PermitLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = myOptions.QueueLimit;
                });
                config.AddFixedWindowLimiter(policyName: Policy.FixedWindow.ToString(), options =>
                {
                    options.PermitLimit = myOptions.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(myOptions.Window);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = myOptions.QueueLimit;
                });
                config.AddSlidingWindowLimiter(policyName: Policy.SlidingWindow.ToString(), options =>
                {
                    options.PermitLimit = myOptions.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(myOptions.Window);
                    options.SegmentsPerWindow = myOptions.SegmentsPerWindow;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = myOptions.QueueLimit;
                });
                config.AddTokenBucketLimiter(policyName: Policy.BucketToken.ToString(), options =>
                {
                    options.TokenLimit = myOptions.TokenLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = myOptions.QueueLimit;
                    options.ReplenishmentPeriod = TimeSpan.FromSeconds(myOptions.ReplenishmentPeriod);
                    options.TokensPerPeriod = myOptions.TokensPerPeriod;
                    options.AutoReplenishment = myOptions.AutoReplenishment;
                });
            }
                );
        }
    }
}
