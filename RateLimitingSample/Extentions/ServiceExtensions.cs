using Microsoft.AspNetCore.RateLimiting;
using RateLimitingSample.Enums;
using RateLimitingSample.Models;
using System.Globalization;
using System.Net;
using System.Threading.RateLimiting;

namespace RateLimitingSample.Extentions
{
    public static class ServiceExtensions
    {
        public static void AddRateLimiterExtension(this IServiceCollection services, IConfiguration configuration)
        {

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

                config.AddPolicy(Policy.UserBasedPolicy.ToString(), context =>
                {

                    if (context.User.Identity?.IsAuthenticated == true)
                    {
                        // UserBased, TokenBucket
                        configuration.GetSection(RateLimitingSettings.UserBasedTokenBucket.ToString()).Bind(limitSettings);
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
                    configuration.GetSection(RateLimitingSettings.UserBasedSlidingWindow.ToString()).Bind(limitSettings);
                    return RateLimitPartition.GetSlidingWindowLimiter("anonymous-user", _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = limitSettings.PermitLimit,
                        Window = TimeSpan.FromSeconds(limitSettings.Window),
                        SegmentsPerWindow = limitSettings.SegmentsPerWindow,
                        QueueLimit = limitSettings.QueueLimit,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });
                });


                config.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, IPAddress>(context =>
                {
                    configuration.GetSection(RateLimitingSettings.RateLimitGlobalFixedWindow.ToString()).Bind(limitSettings);
                    IPAddress? remoteIpAddress = context.Connection.RemoteIpAddress;

                    //if (!IPAddress.IsLoopback(remoteIpAddress!))
                    //{
                    //    return RateLimitPartition.GetFixedWindowLimiter
                    //    (remoteIpAddress!, _ =>
                    //        new FixedWindowRateLimiterOptions
                    //        {
                    //            PermitLimit = limitSettings.PermitLimit,
                    //            Window = TimeSpan.FromSeconds(limitSettings.Window),
                    //            QueueLimit = limitSettings.QueueLimit,
                    //            AutoReplenishment = limitSettings.AutoReplenishment
                    //        });
                    //}

                    return RateLimitPartition.GetNoLimiter(IPAddress.Loopback);
                });

                //Fixed Window
                config.AddFixedWindowLimiter(policyName: Policy.FixedWindowPolicy.ToString(), options =>
                {
                    configuration.GetSection(RateLimitingSettings.FixedWindow.ToString()).Bind(limitSettings);
                    options.PermitLimit = limitSettings.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(limitSettings.Window);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = limitSettings.QueueLimit;
                });
                // Sliding Window
                config.AddSlidingWindowLimiter(policyName: Policy.SlidingWindowPolicy.ToString(), options =>
                {
                    configuration.GetSection(RateLimitingSettings.SlidingWindow).Bind(limitSettings);
                    options.PermitLimit = limitSettings.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(limitSettings.Window);
                    options.SegmentsPerWindow = limitSettings.SegmentsPerWindow;
                    options.QueueLimit = limitSettings.QueueLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });
                // Token Bucket
                config.AddTokenBucketLimiter(policyName: Policy.TokenBucketPolicy.ToString(), options =>
                {
                    configuration.GetSection(RateLimitingSettings.TokenBucket.ToString()).Bind(limitSettings);
                    options.TokenLimit = limitSettings.TokenLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = limitSettings.QueueLimit;
                    options.ReplenishmentPeriod = TimeSpan.FromSeconds(limitSettings.ReplenishmentPeriod);
                    options.TokensPerPeriod = limitSettings.TokensPerPeriod;
                    options.AutoReplenishment = limitSettings.AutoReplenishment;
                });
                // Concurrency
                config.AddConcurrencyLimiter(policyName: Policy.ConcurrencyPolicy.ToString(), options =>
                {
                    configuration.GetSection(RateLimitingSettings.Concurrency.ToString()).Bind(limitSettings);
                    options.PermitLimit = limitSettings.PermitLimit;
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = limitSettings.QueueLimit;
                });

            }
                );
        }
    }
}
