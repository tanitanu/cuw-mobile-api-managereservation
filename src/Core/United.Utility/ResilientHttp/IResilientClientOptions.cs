using System;
using System.Collections.Generic;
using System.Text;
using United.Utility.Config;

namespace United.Utility.Http
{
    public interface IResilientClientOptions
    {
        string BaseUrl { get; set; }
        TimeoutPolicyConfig TimeoutPolicyConfig { get; set; }
        RetryPolicyConfig RetryPolicyConfig { get; set; }
        CircuitBreakerPolicyConfig CircuitBreakerPolicyConfig { get; set; }
    }
}
