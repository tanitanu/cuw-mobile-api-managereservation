using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Utility.Helper
{
    [Serializable()]
    public class LogEntryHelper
    {
        //public long Id { get; set; }

        //public string Guid { get; set; }

        //private string machineName = System.Environment.MachineName;

        //public string MachineName
        //{
        //    get
        //    {
        //        return this.machineName;
        //    }
        //    set
        //    {
        //        this.machineName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
        //    }
        //}

        //public string Action { get; set; }

        //public string MessageType { get; set; }

        //public string Message { get; set; }

        //public string DeviceID { get; set; }

        //public int ApplicationID { get; set; }

        //public string AppVersion { get; set; }

        //public DateTime InsertDateTime { get; set; }

        //public static LogEntry GetLogEntry<T>(string guid, string action, string messageType, int applicationID, string appVersion, string deviceID, T t)
        //{
        //    LogEntry logEntry = new LogEntry();
        //    logEntry.Guid = guid;
        //    logEntry.Action = action;
        //    logEntry.MessageType = messageType;
        //    logEntry.ApplicationID = applicationID;
        //    logEntry.AppVersion = appVersion;
        //    logEntry.DeviceID = deviceID;
        //    try
        //    {
        //        logEntry.Message = XmlSerializerHelper.Serialize<T>(t);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        Trace.WriteLine(ex.Message);
        //    }
        //    return logEntry;
        //}

        //[System.ComponentModel.DefaultValue(false)]
        //public bool IsJSONSave { get; set; }

        //[System.ComponentModel.DefaultValue(false)]
        //public bool LogIt { get; set; }

        public static LogEntry GetLogEntry<T>(string guid, string action, string messageType, int applicationID, string appVersion, string deviceID, T t, bool isJSONSave, bool LogIt)
        {
            return AssignPropertyValues(guid, action, messageType, applicationID, appVersion, deviceID, t, isJSONSave, LogIt);
        }

        private static LogEntry AssignPropertyValues<T>(string guid, string action, string messageType, int applicationID, string appVersion, string deviceID, T t, bool isJSONSave, bool LogIt)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Action = action;
            logEntry.Guid = guid;
            logEntry.MessageType = messageType;
            logEntry.AppVersion = appVersion;
            logEntry.ApplicationID = applicationID;
            logEntry.DeviceID = deviceID;
            logEntry.IsJSONSave = isJSONSave;
            logEntry.LogIt = LogIt;
            try
            {
                if (!logEntry.IsJSONSave)
                {
                    logEntry.Message = XmlSerializerHelper.Serialize<T>(t);
                }
                else
                {
                    try
                    {
                        logEntry.Message = XmlSerializerHelper.Serialize<T>(t);
                    }
                    catch
                    {
                        logEntry.Message = XmlSerializerHelper.Serialize<T>(t);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return logEntry;
        }

        public static LogEntry GetLogEntry<T>(string guid, string action, string messageType, T t)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Guid = guid;
            logEntry.Action = action;
            logEntry.MessageType = messageType;
            logEntry.AppVersion = string.Empty;
            logEntry.DeviceID = string.Empty;
            try
            {
                logEntry.Message = XmlSerializerHelper.Serialize<T>(t);
            }
            catch (System.Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return logEntry;
        }

    }
}
