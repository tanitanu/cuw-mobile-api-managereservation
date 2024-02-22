using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.MPRewards
{
    public class OfferRequestData
    {
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string DepartDateTimeUtc { get; set; }
        public string FlightNumber { get; set; }
        public string Carrier { get; set; }
        public string View { get; set; }
        public SeatFocusRequest FocusRequestData { get; set; }

    }

    public class SeatFocusRequest
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string SeatNumber { get; set; }
        public int LastNameIndex { get; set; }
        public int FirstNameIndex { get; set; }
    }
}
