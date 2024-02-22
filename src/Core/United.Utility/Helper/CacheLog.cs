using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Serilog;

namespace United.Utility.Helper
{
    public enum LogType { INFORMATION, ERROR, WARNING }
    public class CacheLog<T> : ICacheLog<T> where T : class
    {
        private readonly CacheLogWriter _cacheLogWriter;
        private readonly IConfiguration _configuration;

        public CacheLog(CacheLogWriter cacheLogWriter, IConfiguration configuration)
        {
            _cacheLogWriter = cacheLogWriter;
            _configuration = configuration;
        }
        public void LogInformation(string messageTemplate, params object[] values)
        {
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                var logRecord = new LogModel { LogType = LogType.INFORMATION, MessageTemplate = messageTemplate, Values = values };
                if (_configuration.GetValue<bool>("EnableAllLogs"))
                {
                    _cacheLogWriter.InsertLogInformation(logRecord);
                }
                else
                {
                    _cacheLogWriter.AddLog(logRecord);
                }
            }
        }

        public void LogError(string messageTemplate, params object[] values)
        {
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                var logRecord = new LogModel { LogType = LogType.ERROR, MessageTemplate = messageTemplate, Values = values };
                if (_configuration.GetValue<bool>("EnableAllLogs"))
                {
                    _cacheLogWriter.InsertLogError(logRecord);
                }
                else
                {
                    _cacheLogWriter.AddLog(logRecord);
                }
            }
        }

        public void LogWarning(string messageTemplate, params object[] values)
        {
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                var logRecord = new LogModel { LogType = LogType.WARNING, MessageTemplate = messageTemplate, Values = values };
                if (_configuration.GetValue<bool>("EnableAllLogs"))
                {
                    _cacheLogWriter.InsertLogWarning(logRecord);
                }
                else
                {
                    _cacheLogWriter.AddLog(logRecord);
                }
            }
        }

        public void ILoggerInfo(string messageTemplate, params object[] values)
        {
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                var logRecord = new LogModel { LogType = LogType.INFORMATION, MessageTemplate = messageTemplate, Values = values };
                _cacheLogWriter.InsertLogInformation(logRecord);
            }
        }
        public void ILoggerError(string messageTemplate, params object[] values)
        {
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                var logRecord = new LogModel { LogType = LogType.ERROR, MessageTemplate = messageTemplate, Values = values };
                _cacheLogWriter.InsertLogError(logRecord);
            }
        }
        public void ILoggerWarning(string messageTemplate, params object[] values)
        {
            if (!string.IsNullOrEmpty(messageTemplate))
            {
                var logRecord = new LogModel { LogType = LogType.WARNING, MessageTemplate = messageTemplate, Values = values };
                _cacheLogWriter.InsertLogWarning(logRecord);
            }
        }
        public IDisposable BeginTimedOperation(
             string description,
             string transationId = null,
             LogLevel level = LogLevel.Information,
             TimeSpan? warnIfExceeds = null,
             LogLevel levelExceeds = LogLevel.Warning,
             string beginningMessage = TimedOperation.BeginningOperationTemplate,
             string completedMessage = TimedOperation.CompletedOperationTemplate,
             string exceededOperationMessage = TimedOperation.OperationExceededTemplate,
             params object[] propertyValues)
        {
            return null;
            // return _logger.BeginTimedOperation(description, transationId, level, warnIfExceeds, levelExceeds, beginningMessage, completedMessage, exceededOperationMessage, propertyValues);
        }
    }

    public class LogModel
    {
        public LogType LogType { get; set; }
        public string MessageTemplate { get; set; }
        public object[] Values { get; set; }
    }


}
