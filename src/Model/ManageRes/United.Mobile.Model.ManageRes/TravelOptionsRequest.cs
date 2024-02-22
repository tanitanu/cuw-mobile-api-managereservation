using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ManageRes
{
    public class TravelOptionsRequest : MOBRequest
    {
        public string MileagePlusNumber { get; set; }
        public string SessionId { get; set; }
        public string HashKey { get; set; }
        public string Flow { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public string CorrelationId { get; set; }
    }
}
