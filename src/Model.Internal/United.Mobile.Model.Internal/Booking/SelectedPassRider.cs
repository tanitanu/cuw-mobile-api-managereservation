using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;
using United.Mobile.Model.Internal.PassRiders;

namespace United.Mobile.Model.Internal.Booking
{
    public class SelectedPassRider
    {
        public int Age { get; set; }
        public TypeOption Cabin { get; set; }
        public string DayOfContactInformation { get; set; }
        public JumpSeatType JumpSeatType { get; set; }
        public BookingLapChild LapChild { get; set; }
        public PassType PassType { get; set; }
        public string PaxID { get; set; }
        public RelationShip Relationship { get; set; }
        public int SortOrder { get; set; }
        public List<SpecialService> SpecialServices { get; set; }
        public List<SpecialService> SpecialService { get; set; }
        public TravelDocument TravelDocument { get; set; }
        public UpdateTravelDocument UpdateTravelDocument { get; set; }

        public TypeOption KnownTraveler { get; set; } = new TypeOption();
        public TypeOption Redress { get; set; } = new TypeOption();
        public string Gender { get; set; } = string.Empty;
        public string Nationality { get; set; }
        public string Country { get; set; }

    }
    public class UpdateTravelDocument
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public string PhoneCountryCode { get; set; }
        public string Gender { get; set; }
    }

}
