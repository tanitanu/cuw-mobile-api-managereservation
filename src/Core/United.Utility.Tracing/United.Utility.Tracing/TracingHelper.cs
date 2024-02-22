using System;
using Datadog.Trace;
using Microsoft.Extensions.Configuration;

namespace United.Utility.Tracing
{
    public static class TracingHelper
    {
        private static bool _isTracingEnabled;
        public static void InIt(IConfiguration configuration)
        {
            _isTracingEnabled = SetTracingEnabled(configuration);
        }

        public static bool IsTracingEnabled => _isTracingEnabled;

        private static bool SetTracingEnabled(IConfiguration configuration)
        {
            var enabled = true;
            var setting = configuration.GetSection(TracingConstants.Tracing);
            if (setting != null)
            {
                var value = setting[TracingConstants.Enable];
                if (!string.IsNullOrWhiteSpace(value))
                    enabled = Convert.ToBoolean(value);
            }
            return enabled;
        }

        public static Scope GetNewDataDogTracingScope(string scopeName)
        {
            return _isTracingEnabled ? Tracer.Instance.StartActive(scopeName) : null;
        }

        public static Scope DataDogActiveScope => _isTracingEnabled ? Tracer.Instance.ActiveScope : null;
    }
}
