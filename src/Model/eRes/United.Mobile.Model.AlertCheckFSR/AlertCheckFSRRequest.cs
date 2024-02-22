using System;
using United.Mobile.Model.FlightSearchResult;
using Trip = United.Mobile.Model.Internal.Common;


namespace United.Mobile.Model.AlertCheckFSR
{
    public class AlertCheckFSRRequest : RequestBase
    {
        public Trip.Trip Trip { get; set; }
        public string ReturnDate { get; set; }       
        public string TravelTypeCode { get; set; }       
        public string SearchType { get; set; }
    }
}
  