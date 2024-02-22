using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    public class Passenger : Name
    {
        public string MileagePlusNumber { get; set; }
        public bool IsEmployee { get; set; }
        public string EmployeeId { get; set; }
        public string EliteStatusCode { get; set; }
        public BookingClass BookingClass { get; set; }
        public SecuredFlightData SecuredFlightData { get; set; }
        public PassengerSegmentInformation PassengerSegmentInformation { get; set; }
        public string FrequentTravelerType { get; set; } //fqtvTyp": " ",
        public string FrequentTravelerCode { get; set; } //fqtvCd": "FQTV",
        public string FrequentTravelerCarrierCode { get; set; } //fqtvCarrCd": "UA",
        public string FrequentTravelerValidationCode { get; set; } //fqtvVldCd": "05",
        public int LastNamePosition { get; set; } //lNmPos": 1,
        public int FirstNamePosition { get; set; } //fNmPos": 1,
        public bool IsStandBy { get; set; }
        public List<SeatPreferenceOption> SeatPreferences { get; set; } = new List<SeatPreferenceOption>();
        public MOBPNRPassenger TravelersInfo { get; set; }
    }
}
