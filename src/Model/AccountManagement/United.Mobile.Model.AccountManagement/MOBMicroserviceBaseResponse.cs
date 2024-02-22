using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    public class MOBMicroserviceBaseResponse<T>
    {
        public T Data { get; set; }
        public string DateTimeUtc { get; set; } = DateTime.UtcNow.ToString("yyyyMMdd hh:mm:ss");
        public string MachineName { get; set; } = System.Environment.MachineName;
        public long Duration { get; set; }
        public IDictionary<string, ICollection<string>> Errors { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }

        public MOBMicroserviceBaseResponse()
        {
            Errors = new Dictionary<string, ICollection<string>>();
        }

    }
}
