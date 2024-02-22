using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class ShopClassOfService
    {
        public string FareClass { get; set; }

        public string SeatAvailable { get; set; }
    }
}