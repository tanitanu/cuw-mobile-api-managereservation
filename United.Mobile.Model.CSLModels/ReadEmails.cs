using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class ReadEmails : Base
    {
        public List<Email> Emails { get; set; }

        public string CustomerId { get; set; }

        public string LoyaltyId { get; set; }
    }
}
