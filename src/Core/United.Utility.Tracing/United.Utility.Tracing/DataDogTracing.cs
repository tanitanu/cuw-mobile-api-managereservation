using System;
using System.Collections.Generic;
using System.Text;
using United.Ebs.Common.Interfaces;

namespace United.Utility.Tracing
{
    public class DataDogTracing : ITracing
    {
        private readonly IRequestContext _requestContext;
        public DataDogTracing(IRequestContext requestContext)
        {
            _requestContext = requestContext;
        }

        public void AddTraceWithDefaults(string key, string value)
        {
            AddToTrace(key, value);
            AddSession();
            AddToken();
            AddClientId();
        }
        public void Add(string key, string value)
        {
            AddToTrace(key, value);
        }

        public void AddSession()
        {
            var sessionId = _requestContext?.SessionId;
            AddToTrace(TracingConstants.SessionId, sessionId);
        }

        public void AddToken()
        {
            var idToken = _requestContext?.IdToken;
            AddToTrace(TracingConstants.IdToken, idToken);
        }

        public void AddClientId()
        {
            var clientId = _requestContext?.ClientId;
            AddToTrace(TracingConstants.ClientId, clientId);
        }

        private void AddToTrace(string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value)) return;
            var scope = TracingHelper.DataDogActiveScope;
            if (scope != null)
            {
                scope.Span.SetTag(key, value);
            }
        }

        public void SetException(Exception ex)
        {
            if (ex == null) return;
            var scope = TracingHelper.DataDogActiveScope;
            if (scope != null)
            {
                scope.Span.SetException(ex);
            }
        }
    }
}
