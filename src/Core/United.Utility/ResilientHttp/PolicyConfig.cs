namespace United.Utility.Config
{
    public class TimeoutPolicyConfig
    {
        /// <summary>
        /// Timeout seconds
        /// </summary>
        public int Seconds { get; set; } = 30;//3;
    }

    public class RetryPolicyConfig
    {
        /// <summary>
        /// The retry times if request is failed
        /// </summary>
        public int RetryCount { get; set; } = 3;//1;
    }

    public class CircuitBreakerPolicyConfig
    {
        /// <summary>
        /// The retry times before the CircuitBreaker opens
        /// </summary>
        public int AllowExceptions { get; set; } = 3;

        /// <summary>
        /// The sleep duration for the opening circuit breaker, based on seconds
        /// </summary>
        public int BreakDuration { get; set; } = 60;//6;
    }
}
