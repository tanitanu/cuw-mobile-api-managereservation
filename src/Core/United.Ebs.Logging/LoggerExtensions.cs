using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using United.Csl.Ms.Common.Interfaces;
using United.Ebs.Logging.Common;
using United.Ebs.Logging.Enrichers;
using United.Ebs.Logging.Providers;

namespace United.Ebs.Logging
{
    public static class LoggerExtensions
    {
        public static IHttpContextAccessor ContextAccessor { get; set; }

        public static void LogRequest(this ILogger logger, dynamic requestBody, params object[] args)
        {
            IRequestContext reqContext = GetRequestContext();
            if (reqContext == null)
                return;

            IRequestEnricher enricher = GetRequestEnricher();
            if (enricher == null)
                return;

            IOptionsSnapshot<LoggingConfiguration> config = GetLogConfiguration();
            if (config != null && !config.Value.RequestResponseEnabled)
                return;

            enricher.ProcessRequest(requestBody);

            logger.LogInformation(Constants.RequestLog);
        }

        public static void LogResponse(this ILogger logger, dynamic responseBody, double responseTime, params object[] args)
        {
            IRequestEnricher enricher = GetRequestEnricher();
            if (enricher == null)
                return;

            IOptionsSnapshot<LoggingConfiguration> config = GetLogConfiguration();
            if (config != null && !config.Value.RequestResponseEnabled)
                return;

            enricher.ProcessResponse(responseBody, responseTime);

            logger.LogInformation(Constants.ResponseLog);
        }

        private static IRequestEnricher GetRequestEnricher()
        {
            if (ContextAccessor == null)
                return null;

            var reqEnricher = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IRequestEnricher)) as IRequestEnricher;
            if (reqEnricher == null)
                return null;

            return reqEnricher;
        }

        private static IRequestContext GetRequestContext()
        {
            if (ContextAccessor == null)
                return null;

            var reqContext = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IRequestContext)) as IRequestContext;
            if (reqContext == null)
                return null;

            return reqContext;
        }

        private static IOptionsSnapshot<LoggingConfiguration> GetLogConfiguration()
        {
            if (ContextAccessor == null)
                return null;

            var config = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IOptionsSnapshot<LoggingConfiguration>)) as IOptionsSnapshot<LoggingConfiguration>;
            if (config == null)
                return null;

            return config;
        }
    }
}
