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
    public class CacheLogWriter
    {
        private ILogger<CacheLogWriter> _logger;
        private readonly List<LogModel> _SavedObject;

        public CacheLogWriter(ILogger<CacheLogWriter> logger)
        {
            _logger = logger;
            _SavedObject = new List<LogModel>();
        }

        public void AddLog(LogModel log)
        {
            _SavedObject.Add(log);
        }

        public void SaveToLogs()
        {
            try
            {
                if (_SavedObject.Any(_ => _.LogType == LogType.ERROR || _.LogType == LogType.WARNING))
                {
                    var logslist = new List<LogModel>(_SavedObject);
                    foreach (var tempObj in logslist)
                    {
                        try
                        {
                            switch (tempObj.LogType)
                            {
                                case LogType.INFORMATION:
                                    InsertLogInformation(tempObj);
                                    break;
                                case LogType.WARNING:
                                    InsertLogWarning(tempObj);
                                    break;
                                case LogType.ERROR:
                                    InsertLogError(tempObj);
                                    break;
                                default:
                                    InsertLogInformation(tempObj);
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation("Error in adding the list item" + tempObj.MessageTemplate, ex);
                        }
                    }
                    logslist = null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("SaveToLogs {ex}", ex);
            }
            _SavedObject.Clear();
        }

        public void InsertLogInformation(LogModel logModel)
        {
            try
            {
                _logger.LogInformation(logModel.MessageTemplate, logModel.Values ?? null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InsertLogInformation {ex}", ex);
            }
        }
        public void InsertLogWarning(LogModel logModel)
        {
            try
            {
                _logger.LogWarning(logModel.MessageTemplate, logModel.Values ?? null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InsertLogWarning {ex}", ex);
            }
        }
        public void InsertLogError(LogModel logModel)
        {
            try
            {
                _logger.LogError(logModel.MessageTemplate, logModel.Values ?? null);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("InsertLogError {ex}", ex);
            }
        }
        private void InsertBiginTimedOperation(LogModel logModel)
        {
        }
        public IDisposable BeginTimedOperation(
             string description,
             string transationId = null,
             LogLevel level = LogLevel.Information,
             TimeSpan? warnIfExceeds = null,
             LogLevel levelExceeds = LogLevel.Warning,
             string beginningMessage = TimedOperation.BeginningOperationTemplate, string completedMessage = TimedOperation.CompletedOperationTemplate, string exceededOperationMessage = TimedOperation.OperationExceededTemplate,
             params object[] propertyValues)
        {
            return _logger.BeginTimedOperation(description, transationId, level, warnIfExceeds, levelExceeds, beginningMessage, completedMessage, exceededOperationMessage, propertyValues);
        }
    }
}

