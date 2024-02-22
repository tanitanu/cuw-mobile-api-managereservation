using System;

namespace United.Mobile.Model.Common
{
    public class SessionResponse
    {
        public bool Succeed { get; set; }
        public string SessionId { get; set; }
        public dynamic Data { get; set; }
        public string DateTimeUtc { get; set; } = DateTime.UtcNow.ToString();
        public string MachineName { get; set; } = System.Environment.MachineName;
        public long Duration { get; set; }
    }
}
