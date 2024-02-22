using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopFlightInfo
    {
        public string ActualArrivalDateTime { get; set; } = string.Empty;
        public string ActualDepartureDateTime { get; set; } = string.Empty;
        public string EstimatedArrivalDateTime { get; set; } = string.Empty;
        public string EstimatedDepartureDateTime { get; set; } = string.Empty;
        public string MinutesDelayed { get; set; } = string.Empty;
        public string ScheduledArrivalDataTime { get; set; } = string.Empty;
        public string ScheduledDepartureDateTime { get; set; } = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
        public string StatusMessage { get; set; } = string.Empty;
    }
}
