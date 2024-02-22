using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPTraveler
    {
        //TODO Sathwika
        public string SHARESPosition { get; set; } = string.Empty;

        //public Ticket Ticket { get; set; }

        public long CustomerId { get; set; }

        public string Key { get; set; } = string.Empty;

        //public MOBSHOPLoyaltyProgramProfile LoyaltyProgramProfile { get; set; }

        public List<MOBPrefSpecialRequest> SpecialRequests { get; set; }

        public Person Person { get; set; }

        public bool IsProfileOwner { get; set; }

        public bool IsTSAFlagOn { get; set; }

        public bool IsFQTVNameMismatch { get; set; }

        public string FQTVNameMismatch { get; set; } = string.Empty;

        public string TravelerTypeCode { get; set; } = string.Empty;

        public int PaxIndex { get; set; }

        public string IdType { get; set; } = string.Empty;

        public List<MOBSeat> Seats { get; set; }

        public List<MOBPrefAirPreference> AirPreferences { get; set; }

        public List<PrefContact> Contacts { get; set; }

        public MOBSHOPTraveler()
        {
            SpecialRequests = new List<MOBPrefSpecialRequest>();
            Seats = new List<MOBSeat>();
            AirPreferences = new List<MOBPrefAirPreference>();
            Contacts = new List<PrefContact>();
        }
    }
}
