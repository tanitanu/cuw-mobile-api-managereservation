using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;
using System.Text;
using United.Ebs.Logging.Providers;
using United.Csl.Ms.Common.Interfaces;

namespace United.Ebs.Logging.Enrichers
{
    public class LogEventEnricher : ILogEventEnricher
    {
        public static IHttpContextAccessor ContextAccessor { get; set; }

        // Added by Ashrith
        public static List<KeyValuePair<string, dynamic>> appEnricherKeyValues { get; set; }

        // Added by Ashrith
        public static List<KeyValuePair<string, dynamic>> contextEnricherKeyValues { get; set; }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent == null || logEvent.MessageTemplate.Text == Common.Constants.EbsLoggingException)
                return;
            try
            {
                if (logEvent.Level == LogEventLevel.Information && logEvent.Properties["RequestPath"].ToString().ToLower().Contains("/api/healthcheck"))
                {
                    return;
                }
            }
            catch { }

            try
            {
                var options = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IOptionsSnapshot<LoggingConfiguration>)) as IOptionsSnapshot<LoggingConfiguration>;

                RemoveDefaultProperties(logEvent);

                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(Common.Constants.StandardAttributes.LoginStamp, GetLogTime(options?.Value?.DateFormat)));
                // Added by Ashrith
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(Common.Constants.StandardAttributes.RenderedMessage, logEvent.MessageTemplate.Render(logEvent.Properties)));
                LoadAppEnricherAttributes(logEvent, propertyFactory);
                // Added by Ashrith
                LoadAppEnricherAttributesWorkerService(logEvent, propertyFactory);
                // Added by Ashrith
                LoadContextEnricherAttributes(logEvent, propertyFactory);
                LoadRequestContextAttributes(logEvent, propertyFactory);
                LoadRequestEnricherAttributes(logEvent, propertyFactory);
                LoadRequestResponseAttributes(logEvent, propertyFactory, options);
                LoadAdditionalAttributes(logEvent, propertyFactory);
                LoadExceptionAttributes(logEvent, propertyFactory);
                
                // Final cleanup
                CleanUpAttributes(logEvent);
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty(Common.Constants.EbsLoggingException + "Enrich", ex.ToString()));
            }
        }

        private void RemoveDefaultProperties(LogEvent logEvent)
        {
            if (logEvent.Properties == null || logEvent.Properties.Count <= 0)
                return;

            // These properties we will remove
            logEvent.RemovePropertyIfPresent(Common.Constants.SerilogAttributes.ActionId);
            logEvent.RemovePropertyIfPresent(Common.Constants.SerilogAttributes.ConnectionId);
            logEvent.RemovePropertyIfPresent(Common.Constants.SerilogAttributes.SpanId);
            logEvent.RemovePropertyIfPresent(Common.Constants.SerilogAttributes.ParentId);
            logEvent.RemovePropertyIfPresent(Common.Constants.SerilogAttributes.TraceId);
            logEvent.RemovePropertyIfPresent(Common.Constants.SerilogAttributes.EventId);
        }

        private void CleanUpAttributes(LogEvent logEvent)
        {
            // These are special cases that need to be cleaned up
            logEvent.RemovePropertyIfPresent(Common.Constants.Attribs);
        }
        private void LoadAppEnricherAttributes(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            var appEnricher = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IApplicationEnricher)) as IApplicationEnricher;
            if (appEnricher == null)
                return;

            try
            {
                foreach (var prop in appEnricher.KeyValues)
                {
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(prop.Key, prop.Value));
                }
                //below properties added to filter the duplicate logs at opensearch
                logEvent.AddOrUpdateProperty(factory.CreateProperty("uid", Guid.NewGuid().ToString()));
                logEvent.AddOrUpdateProperty(factory.CreateProperty("logid", Guid.NewGuid().ToString()));
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadAppEnricherAttributes", ex.ToString()));
            }
        }

        // Added by Ashrith
        private void LoadAppEnricherAttributesWorkerService(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            if (appEnricherKeyValues == null)
                return;

            try
            {
                foreach (var prop in appEnricherKeyValues)
                {
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(prop.Key, prop.Value));
                }
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadAppEnricherAttributes", ex.ToString()));
            }
        }

        private void LoadRequestContextAttributes(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            var reqContext = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IRequestContext)) as IRequestContext;
            if (reqContext == null)
                return;

            try
            {
                if (reqContext.RequestId != null)
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.RequestHeaderAttributes.RequestId, reqContext.RequestId));

                if (reqContext.ClientId != null)
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.RequestHeaderAttributes.ClientId, reqContext.ClientId));

                if (reqContext.SessionId != null)
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.RequestHeaderAttributes.SessionId, reqContext.SessionId));

                if (reqContext.CallerServiceName != null)
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.RequestHeaderAttributes.CallerServiceName, reqContext.CallerServiceName));
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadRequestContextAttributes", ex.ToString()));
            }
        }

        private void LoadRequestEnricherAttributes(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            var reqEnricher = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IRequestEnricher)) as IRequestEnricher;
            if (reqEnricher == null)
                return;

            try
            {
                foreach (var prop in reqEnricher.KeyValues)
                {
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(prop.Key, prop.Value));
                }
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadRequestEnricherAttributes", ex.ToString()));
            }
        }

        private void LoadRequestResponseAttributes(LogEvent logEvent, ILogEventPropertyFactory factory, IOptionsSnapshot<LoggingConfiguration> options)
        {
            var reqEnricher = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IRequestEnricher)) as IRequestEnricher;
            if (reqEnricher == null)
                return;

            if (options == null)
                return;

            if (reqEnricher.RequestInfo == null && reqEnricher.ResponseInfo == null)
                return;

            if (!options.Value.RequestResponseEnabled)
                return;

            try
            {
                if (logEvent.Level == LogEventLevel.Fatal || logEvent.MessageTemplate.Text == Common.Constants.RequestLog || logEvent.MessageTemplate.Text == Common.Constants.ResponseLog)
                {
                    foreach (var prop in reqEnricher.RequestInfo)
                    {
                        logEvent.AddOrUpdateProperty(factory.CreateProperty(prop.Key, prop.Value));
                    }
                }

                if (logEvent.Level == LogEventLevel.Fatal || logEvent.MessageTemplate.Text == Common.Constants.ResponseLog)
                {
                    foreach (var prop in reqEnricher.ResponseInfo)
                    {
                        logEvent.AddOrUpdateProperty(factory.CreateProperty(prop.Key, prop.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadRequestResponseAttributes", ex.ToString()));
            }
        }

        private void LoadAdditionalAttributes(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            LogEventPropertyValue propValue = logEvent.Properties.GetValueOrDefault(Common.Constants.Attribs);
            if (propValue == null)
                return;

            DictionaryValue attribs = propValue as DictionaryValue;
            if (attribs == null)
                return;

            try
            {
                foreach (var attrib in attribs.Elements)
                {
                    object key = GetPropertyValue(attrib.Key);
                    object value = GetPropertyValue(attrib.Value);
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(key.ToString(), value));
                }
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadAdditionalAttributes", ex.ToString()));
            }
        }

        // Added by Ashrith
        private void LoadContextEnricherAttributes(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            if (contextEnricherKeyValues == null)
                return;

            try
            {
                foreach (var prop in contextEnricherKeyValues)
                {
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(prop.Key, prop.Value));
                }
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadRequestEnricherAttributes", ex.ToString()));
            }
        }

        private void LoadExceptionAttributes(LogEvent logEvent, ILogEventPropertyFactory factory)
        {
            if (logEvent.Exception == null)
                return;

            try
            {
                int exceptionLength = -1, stackTraceLength = -1;
                var options = ContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IOptionsSnapshot<LoggingConfiguration>)) as IOptionsSnapshot<LoggingConfiguration>;
                if (options != null)
                {
                    exceptionLength = options.Value.InnerExceptionLength;
                    stackTraceLength = options.Value.StackTraceLength;
                }

                string exception = CreateChainedInnerException(logEvent.Exception, exceptionLength);
                if (exception != null && exception.Length > 0)
                {
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.ErrorAttributes.InnerException, exception));
                }
                string stacktrace = CreateStackTrace(logEvent.Exception, stackTraceLength);
                if (stacktrace != null && stacktrace.Length > 0)
                {
                    logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.ErrorAttributes.StackTrace, stacktrace));
                }
            }
            catch (Exception ex)
            {
                logEvent.AddOrUpdateProperty(factory.CreateProperty(Common.Constants.EbsLoggingException + "LoadExceptionAttributes", ex.ToString()));
            }
        }

        private string CreateChainedInnerException(Exception ex, int maxLength)
        {
            string result = string.Empty;

            try
            {
                var innerEx = ex?.InnerException;
                var chainedEx = new StringBuilder();
                var maxCount = 20;
                var count = 0;
                while (innerEx != null && count < maxCount)
                {
                    chainedEx.Append(" " + innerEx);
                    innerEx = innerEx.InnerException;
                    count++;
                }

                result = chainedEx.Length > maxLength ? chainedEx.ToString(0, maxLength) : chainedEx.ToString();
            }
            catch (Exception cex)
            {
                result = Common.Constants.EbsLoggingException + "CreateChainedInnerException =" + cex.ToString();
            }

            return result;
        }

        private string CreateStackTrace(Exception ex, int maxLength)
        {
            if (ex == null || ex.StackTrace == null)
                return null;

            if (maxLength == -1)
                return ex.StackTrace;

            try
            {
                return (ex.StackTrace.Length > maxLength) ? ex.StackTrace.Substring(ex.StackTrace.Length - maxLength, maxLength) : ex.StackTrace;
            }
            catch (Exception cex)
            {
                return Common.Constants.EbsLoggingException + "CreateStackTrace =" + cex.ToString();
            }
        }

        private string GetLogTime(string format)
        {
            return DateTime.UtcNow.ToString(format);
        }

        private dynamic GetPropertyValue(LogEventPropertyValue propValue)
        {
            ScalarValue value = propValue as ScalarValue;
            if (value == null)
                return string.Empty;

            return value.Value;

        }
    }
}
