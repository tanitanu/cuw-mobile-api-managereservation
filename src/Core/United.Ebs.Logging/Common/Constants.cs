using System.Diagnostics.CodeAnalysis;

namespace United.Ebs.Logging.Common
{
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        public static class StandardAttributes
        {
            public const string LogLevel = "LogLevel";
            public const string Source = "Source";
            public const string EventId = "EventId";
            public const string EventType = "EventType";
            public const string LoginStamp = "LogTime";
            public const string StandardAttributesDescription = "Description";
            public const string Message = "Message";
            public const string RenderedMessage = "RenderedMessage";
        }

        public static class DiagnosticAttributes
        {
            public const string FileName = "LogFileName";
            public const string LineNumber = "LogLineNumber";
            public const string OperationName = "LogOperationName";
        }

        public static class ErrorAttributes
        {
            public const string ErrorCode = "ErrorCode";
            public const string ErrorDescription = "ErrorDescription";
            public const string StackTrace = "StackTrace";
            public const string InnerException = "InnerException";
            public const string LineNumber = "LineNumber";
        }

        public static class TransientAttributes
        {
            public const string StateInfos = "StateInfos";
        }

        public static class RequestPayloadAttributes
        {
            public const string RequestHeader = "RequestHeader";
            public const string RequestMethod = "RequestMethod";
            public const string RequestPath = "RequestPath";
            public const string Request = "Request";
            public const string RefererUri = "Referer";
        }

        public static class ResponsePayloadAttributes
        {
            public const string ResponseHeader = "ResponseHeader";
            public const string Response = "Response";
            public const string HttpStatusCode = "HttpStatusCode";
            public const string ResponseTime = "ResponseTime";
        }

        public static class RequestHeaderAttributes
        {
            public const string CorrelationId = "CorrelationId";
            public const string RequestId = "RequestId";
            public const string ServiceRequestId = "ServiceRequestId";
            public const string SessionId = "SessionId";
            public const string ClientId = "ClientId";
            public const string CallerServiceName = "CallerServiceId";
            public const string ClientIpAddress = "ClientIpAddress";
            public const string ClientReferrerUri = "ClientReferrerUri";
            public const string IdToken = "X-IDTOKEN";
            public const string TransactionId = "X-Transaction-ID";
        }

        public static class ServiceRuntimeAttributes
        {
            public const string HostServerName = "HostServerName"; // Kubernetes node name available
            public const string User = "User";
        }

        public static class ServiceConfigurationAttributes
        {
            public const string ServiceVersion = "ServiceVersion";
            public const string ServiceApplicationId = "ServiceApplicationId";
            public const string ServiceApplicationName = "ServiceApplicationName";
        }

        public static class ServiceMetricAttributes
        {
            public const string ServiceResponseTime = "TotalResponseTime";
            public const string OperationResponseTime = "OperationResponseTime";
        }

        public static class SerilogAttributes
        {
            public const string SourceContext = "SourceContext";
            public const string SpanId = "SpanId";
            public const string ParentId = "ParentId";
            public const string TraceId = "TraceId";
            public const string ConnectionId = "ConnectionId";
            public const string EventId = "EventId";
            public const string ActionId = "ActionId";
        }

        public static readonly string MessageKey = "message";
        public static readonly string ExceptionMessage = ". Exception: ";
        public static readonly string CallerMemberName = "CallerMemberName";
        public static readonly string CallerFileName = "CallerFileName";
        public static readonly string CallerLineNumber = "CallerLineNumber";
        public static readonly string ActionName = "ActionName";
        public static readonly string StateInfo = "StateInfo";        
        public static readonly string Description = "Description";
        public static readonly string MatchStringForFlush = "Request finished";
        public static readonly string ExceptionTitle = ". Exception.Message : ";
        public static readonly string Attribs = "Attribs";
        public static readonly string AttribsTemplate = "{Attribs}";

        public static readonly string RequestLog = "Request Log";
        public static readonly string ResponseLog = "Response Log";
        public static readonly string EbsLoggingException = "EBS Logger Exception: ";
    }
}
