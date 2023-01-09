using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using RateLimitingSample.Enums;
using RateLimitingSample.Models;
using System.Globalization;
using System.Net.Http;
using System.Threading.RateLimiting;

namespace RateLimitingSample.Extentions
{
    public static class ServiceExtensions
    {
        public static void AddRateLimiterExtension(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<RateLimitingSettings>(
                configuration.GetSection(RateLimitingSettings.UserBasedTokenBucket));

            var limitSettings = new RateLimitingSettings();

            services.AddRateLimiter(config =>
            {
                // On Rejected
                config.OnRejected = (context, cancellationToken) =>
                {
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                    {
                        context.HttpContext.Response.Headers.RetryAfter = ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);
                    }

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.HttpContext.RequestServices.GetService<ILoggerFactory>()?
                    .CreateLogger("Microsoft.AspNetCore.RateLimitingMiddleware")
                    .LogWarning("OnRejected: {RequestPath}", context.HttpContext.Request.Path);

                    return new ValueTask();
                };

                // example of policy based on login user name

                config.AddPolicy(Policy.UserBasedPolicy.ToString(), context => { 

                if (context.User.Identity?.IsAuthenticated == true) 
                    {
                        // UserBased, TokenBucket
                        configuration.GetSection(RateLimitingSettings.UserBasedTokenBucket).Bind(limitSettings);
                        return RateLimitPartition.GetTokenBucketLimiter(context.User.Identity.Name!, _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = limitSettings.TokenLimit,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = limitSettings.QueueLimit,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(limitSettings.ReplenishmentPeriod),
                            TokensPerPeriod = limitSettings.TokensPerPeriod,
                            AutoReplenishment = limitSettings.AutoReplenishment,
                        });
                    }
                    // UserBased, SlidingWindow
                    configuration.GetSection(RateLimitingSettings.UserBasedSlidingWindow).Bind(limitSettings);
                    return RateLimitPartition.GetSlidingWindowLimiter("anonymous-user", _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = limitSettings.PermitLimit,
                        Window = TimeSpan.FromSeconds(limitSettings.Window),
                        SegmentsPerWindow = limitSettings.SegmentsPerWindow,
                        QueueLimit = limitSettings.QueueLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
                });

                // Global, FixedWindow
                configuration.GetSection(RateLimitingSettings.RateLimitGlobalFixedWindow).Bind(limitSettings);
                config.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                        factory: partition => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = limitSettings.PermitLimit,
                            Window = TimeSpan.FromSeconds(limitSettings.Window),
                            QueueLimit = limitSettings.QueueLimit,
                            AutoReplenishment = limitSettings.AutoReplenishment
                        }));

                //Fixed Window
                configuration.GetSection(RateLimitingSettings.FixedWindow).Bind(limitSettings);
                config.AddFixedWindowLimiter(policyName: Policy.FixedWindowPolicy.ToString(), options =>
                {
                    options.PermitLimit = limitSettings.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(limitSettings.Window);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = limitSettings.QueueLimit;
                });
                // Sliding Window
                configuration.GetSection(RateLimitingSettings.SlidingWindow).Bind(limitSettings);
                config.AddSlidingWindowLimiter(policyName: Policy.SlidingWindowPolicy.ToString(), options =>
                {
                    options.PermitLimit = limitSettings.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(limitSettings.Window);
                    options.SegmentsPerWindow = limitSettings.SegmentsPerWindow;
                    options.QueueLimit = limitSettings.QueueLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
                // Token Bucket
                configuration.GetSection(RateLimitingSettings.TokenBucket).Bind(limitSettings);
                config.AddTokenBucketLimiter(policyName: Policy.TokenBucketPolicy.ToString(), options =>
                {
                    options.TokenLimit = limitSettings.TokenLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = limitSettings.QueueLimit;
                    options.ReplenishmentPeriod = TimeSpan.FromSeconds(limitSettings.ReplenishmentPeriod);
                    options.TokensPerPeriod = limitSettings.TokensPerPeriod;
                    options.AutoReplenishment = limitSettings.AutoReplenishment;
                });
                // Concurrency
                configuration.GetSection(RateLimitingSettings.Concurrency).Bind(limitSettings);
                config.AddConcurrencyLimiter(policyName: Policy.ConcurrencyPolicy.ToString(), options =>
                {
                    options.PermitLimit = limitSettings.PermitLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = limitSettings.QueueLimit;
                });

            }
                );
        }
    }
}
