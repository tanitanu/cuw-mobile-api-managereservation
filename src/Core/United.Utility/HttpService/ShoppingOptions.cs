using System;
using System.Collections.Generic;
using System.Text;

namespace United.Utility.HttpService
{
    public class ShoppingOptions
    {
        public const string ConfigNode = nameof(ShoppingOptions);
        public string Url { get; set; }
        public double TimeOut { get; set; }
        public int RetryCount { get; set; }
        public int CircuitBreakerAllowExceptions { get; set; }
        public int CircuitBreakerBreakDuration { get; set; }

    }
}
