using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model.Internal.Common
{
    [Serializable]
    public class SQLDBInfo
    {
        [Required]
        public bool Succeed { get; set; }
        public string TraceID { get; set; }
        public string StoreProcName { get; set; }
        public List<string> Data { get; set; }
        public string DateTimeUtc { get; set; } = DateTime.UtcNow.ToString();
        public string MachineName { get; set; } = System.Environment.MachineName;
        public long Duration { get; set; }
        public string Environment { get; set; } = "DEV";

        public IDictionary<string, ICollection<string>> Errors { get; set; }
        public SQLDBInfo()
        {
            Data = new List<string>();
            Errors = new Dictionary<string, ICollection<string>>();
        }
    }
}