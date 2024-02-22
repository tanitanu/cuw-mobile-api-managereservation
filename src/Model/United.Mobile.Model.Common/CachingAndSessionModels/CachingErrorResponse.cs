using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common.CachingAndSessionModels
{
    public class CachingErrorResponse
    {
        public string type { get; set; } = string.Empty;
        public string status { get; set; } = string.Empty;
        public string title { get; set; } = string.Empty;
        public string traceId { get; set; } = string.Empty;

    }
}
