using System;
using System.Collections.Generic;

namespace United.Mobile.Model.BagTracking
{
    [Serializable]
    public class Passenger
    {
        private string id = string.Empty;

        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string GivenName { get; set; } = string.Empty;
        public string SirName { get; set; } = string.Empty;
        public List<LoyaltyProgramProfile> LoyaltyProgramProfiles { get; set; }
        public List<string> BagTags { get; set; }
        public PassengerItinerary Itinerary { get; set; }
        public Passenger()
        {
            //LoyaltyProgramProfiles = new List<LoyaltyProgramProfile>();
            //BagTags = new List<string>();
        }

    }
}
