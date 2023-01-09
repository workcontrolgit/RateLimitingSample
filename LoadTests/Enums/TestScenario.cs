using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadTests.Enums
{
    public enum TestScenario
    {
        UserBasedRateLimit,
        ConcurrencyRateLimit,
        FixedWindowRateLimit,
        SlidingWindowRateLimit,
        TokenBucketRateLimit,
        GlobalRateLimit,
        DisabledRateLimit,
    }
}
