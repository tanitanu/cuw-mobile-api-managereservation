using System;
using System.Collections.Generic;
using System.Text;

namespace United.Ebs.Logging.Models
{
    public class RequestHeaderInfo
    {
        public string IdToken { get; set; }
        public string RequestId { get; set; }
        public string ClientId { get; set; }
        public string SessionId { get; set; }
        public string CallerServiceName { get; set; }
    }
}
