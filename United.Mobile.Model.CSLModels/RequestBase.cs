using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public abstract class RequestBase
    {
        public string RecordLocator { get; set; }

        public string CustomerId { get; set; }

        public string LoyaltyId { get; set; }

        public string CartId { get; set; }

    }
}
