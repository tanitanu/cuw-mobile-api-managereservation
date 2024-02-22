using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace United.Mobile.Model
{
    [Serializable]
    public class Response<T> : IResponse<T>
    {
        [Required]
        public T Data { get; set; }
        [Required]
        public string DateTimeUtc { get; set; } = DateTime.UtcNow.ToString("yyyyMMdd hh:mm:ss");
        [Required]
        public string MachineName { get; set; } = System.Environment.MachineName;
        [Required]
        public long Duration { get; set; }
        public IDictionary<string, ICollection<string>> Errors { get; set; }
        public IDictionary<string, string> _Errors { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public string TransactionId { get; set; }

        public Response()
        {
            Errors = new Dictionary<string, ICollection<string>>();
            _Errors = new Dictionary<string, string>();
        }
    }
}