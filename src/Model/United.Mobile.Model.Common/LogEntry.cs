using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class LogEntry
    {
        public long Id { get; set; }

        public string Guid { get; set; }

        private string machineName = System.Environment.MachineName;

        public string MachineName { get; set; } = string.Empty;

        public string Action { get; set; } = string.Empty;

        public string MessageType { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public string DeviceID { get; set; } = string.Empty;

        public int ApplicationID { get; set; }

        public string AppVersion { get; set; } = string.Empty;

        public DateTime InsertDateTime { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool IsJSONSave { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool LogIt { get; set; }

    }
}