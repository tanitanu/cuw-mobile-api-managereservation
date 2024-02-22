using System;

namespace United.Mobile.Model.UnitedClub.Internal
{
    [Serializable()]
    public class LogEntry
    {
        public long Id { get; set; }

        public string Guid { get; set; }

        private string machineName = System.Environment.MachineName;

        public string MachineName
        {
            get
            {
                return this.machineName;
            }
            set
            {
                this.machineName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Action { get; set; }

        public string MessageType { get; set; }

        public string Message { get; set; }

        public string DeviceID { get; set; }

        public int ApplicationID { get; set; }

        public string AppVersion { get; set; }

        public DateTime InsertDateTime { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool IsJSONSave { get; set; }

        [System.ComponentModel.DefaultValue(false)]
        public bool LogIt { get; set; }

    }
}
