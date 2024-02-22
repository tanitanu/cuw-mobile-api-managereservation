using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class ReadPhones : Base
    {
        public List<Phone> Phones { get; set; }


        public string CustomerId { get; set; }


        public string LoyaltyId { get; set; }
    }
}
