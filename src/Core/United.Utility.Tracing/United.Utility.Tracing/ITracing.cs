using System;
using System.Collections.Generic;
using System.Text;
using Datadog.Trace;

namespace United.Utility.Tracing
{
    public interface ITracing
    {
        void AddTraceWithDefaults(string key, string value);

        void Add(string key, string value);

        void AddToken();

        void AddSession();

        void SetException(Exception ex);

    }
}
