using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKTraveler
    {
        public bool IsEPlusSubscriber { get; set; }


        public string SHARESPosition { get; set; } = string.Empty;

        public Ticket Ticket { get; set; }

        public long CustomerId { get; set; }

        public string Key { get; set; } = string.Empty;

        public MOBBKLoyaltyProgramProfile LoyaltyProgramProfile { get; set; }

        public List<PrefSpecialRequest> SpecialRequests { get; set; }

        public MOBBKPerson Person { get; set; }

        public bool IsProfileOwner { get; set; }

        public bool IsTSAFlagOn { get; set; }

        public bool IsFQTVNameMismatch { get; set; }

        public string FQTVNameMismatch { get; set; }

        public string TravelerTypeCode { get; set; } = string.Empty;

        public int PaxIndex { get; set; }

        public string IdType { get; set; } = string.Empty;

        public List<Seat> Seats { get; set; }

        public List<MOBPrefAirPreference> AirPreferences { get; set; }

        public List<PrefContact> Contacts { get; set; }

        public bool ShowSeatFocus { get; set; } = false;
    }
}
