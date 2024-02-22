using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class ReadAddresses : Base
    {
        public List<Address> Addresses { get; set; }
        public string CustomerId { get; set; }
        public string LoyaltyId { get; set; }
    }
}
