using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Utility.Serilog;

namespace United.Utility.Helper
{
    public interface ICacheLog //< out T> where T : class
    {
        void LogInformation(string messageTemplate, params object[] Values);
        void LogError(string messageTemplate, params object[] Values);
        void LogWarning(string messageTemplate, params object[] Values);
        void ILoggerInfo(string messageTemplate, params object[] values);
        void ILoggerError(string messageTemplate, params object[] values);
        void ILoggerWarning(string messageTemplate, params object[] values);
        IDisposable BeginTimedOperation(
             string description,
             string transationId = null,
             LogLevel level = LogLevel.Information,
             TimeSpan? warnIfExceeds = null,
             LogLevel levelExceeds = LogLevel.Warning,
             string beginningMessage = TimedOperation.BeginningOperationTemplate, string completedMessage = TimedOperation.CompletedOperationTemplate, string exceededOperationMessage = TimedOperation.OperationExceededTemplate,
             params object[] propertyValues);
    }


    public interface ICacheLog<out T> : ICacheLog
    {

    }
}