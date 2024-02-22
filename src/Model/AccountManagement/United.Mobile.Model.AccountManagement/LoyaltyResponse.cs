using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class LoyaltyResponse
    {
        public bool AlreadyEnrolled { get; set; }
        public long CustomerId { get; set; }
        public string LoyaltyId { get; set; }
        //public List<ResponseTime> ResponseTimes { get; set; }
        public string ServiceName { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set; }
    }
}
