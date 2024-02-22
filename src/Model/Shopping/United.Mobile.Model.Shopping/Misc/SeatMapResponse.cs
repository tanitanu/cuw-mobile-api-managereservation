using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class SeatMapResponse : MOBResponse
    {
        public SeatMapResponse()
            : base()
        {
        }

        public SeatMapRequest SeatMapRequest { get; set; }

        public string SessionId { get; set; } = string.Empty;
        public string MarketingCarrierCode { get; set; } = string.Empty;
        public string OperatingCarrierCode { get; set; } = string.Empty;
        public string FlightDate { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
        public string DepartureAirportCode { get; set; } = string.Empty;
        public string ArrivalAirportCode { get; set; } = string.Empty;
        public MOBSeatMap SeatMap { get; set; } 
        public SHOPOnTimePerformance OnTimePerformance { get; set; } 
    }
}
