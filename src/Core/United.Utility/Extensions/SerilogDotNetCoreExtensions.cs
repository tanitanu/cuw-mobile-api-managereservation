using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Context;
using System;
using System.Collections.Generic;
using System.Text;
using United.Utility.Serilog;

namespace Microsoft.Extensions.Logging
{
    public static class SerilogDotNetCoreExtensions
    {
        public static IDisposable BeginTimedOperation(
           this Microsoft.Extensions.Logging.ILogger logger,
           string description,
           string transationId = null,
           LogLevel level = LogLevel.Information,
           TimeSpan? warnIfExceeds = null,
           LogLevel levelExceeds = LogLevel.Warning,
           string beginningMessage = TimedOperation.BeginningOperationTemplate, string completedMessage = TimedOperation.CompletedOperationTemplate, string exceededOperationMessage = TimedOperation.OperationExceededTemplate,
           params object[] propertyValues)
        {
            object operationIdentifier = transationId;

            if (string.IsNullOrEmpty(transationId))
                operationIdentifier = Guid.NewGuid();

            return new TimedOperation(logger, level, warnIfExceeds, operationIdentifier, description, levelExceeds, beginningMessage, completedMessage, exceededOperationMessage, propertyValues);
        }

        public static void AddMobileHeaderProperties(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.Use(async (httpContext, next) =>
            {

                foreach (var headerText in United.Mobile.Model.Constants.HeaderTextList)
                {
                    LogContext.PushProperty(headerText, httpContext.Request.Headers[headerText]);
                }

                await next.Invoke();
            });

            loggerFactory.AddSerilog();
        }

    }
}
