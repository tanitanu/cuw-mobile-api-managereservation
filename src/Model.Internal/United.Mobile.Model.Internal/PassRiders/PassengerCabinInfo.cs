using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.PassRiders
{
    public class PassengerCabinInfo
    {
        public string PassengerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string FlightNumber { get; set; }
        public string Origin { get; set; } 
        public string Destination { get; set; }
        public string Position { get; set; } 
        public string PassClass { get; set; }
        public TypeOption Cabin { get; set; }
        public PassType PassType { get; set; } 
        public List<SpecialService> SpecialService { get; set; }
        public BookingLapChild LapChild { get; set; }
        public PassengerCabinInfo()
        {
            SpecialService = new List<SpecialService>();
        }
    }
}
