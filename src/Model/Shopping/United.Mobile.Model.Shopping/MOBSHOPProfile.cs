using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPProfile
    {
        public bool IsActiveProfile { get; set; } 

        public string AirportCode { get; set; } = string.Empty;

        public string AirportNameLong { get; set; } = string.Empty;

        public string AirportNameShort { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public int ProfileId { get; set; } 

        public List<ProfileMember> ProfileMembers { get; set; }

        public int ProfileOwnerId { get; set; } 

        public string ProfileOwnerKey { get; set; } = string.Empty;

        public string QuickCreditCardKey { get; set; } = string.Empty;

        public string QuickCreditCardNumber { get; set; } = string.Empty;

        public int QuickCustomerId { get; set; } 

        public string QuickCustomerKey { get; set; } = string.Empty;

        public List<ProfileTraveler> Travelers { get; set; } 

        public MOBName OwnerName { get; set; } 

        public string MileagePlusNumber { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public bool IsOneTimeProfileUpdateSuccess { get; set; } 

        public bool IsProfileOwnerTSAFlagON { get; set; } 

        public List<MOBTypeOption> DisclaimerList { get; set; }

        public MOBSHOPProfile()
        {
            DisclaimerList = new List<MOBTypeOption>();
            Travelers = new List<ProfileTraveler>();
            ProfileMembers = new List<ProfileMember>();
        }
    }
}
