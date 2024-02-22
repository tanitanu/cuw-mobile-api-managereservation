using United.Utility.Config;

namespace United.Utility.Http
{
    public class ResilientClientOpitons : IResilientClientOptions
    {
        public string BaseUrl { get; set; }
        public TimeoutPolicyConfig TimeoutPolicyConfig { get; set; }
        public RetryPolicyConfig RetryPolicyConfig { get; set; }
        public CircuitBreakerPolicyConfig CircuitBreakerPolicyConfig { get; set; }
    }
}