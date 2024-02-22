using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopTrip
    {

        public string BBXCellIdSelected { get; set; } = string.Empty;

        public string BBXSession { get; set; } = string.Empty;


        public string BBXSolutionSetId { get; set; } = string.Empty;

        public string CabinType { get; set; } = string.Empty;


        public string DepartDate { get; set; } = string.Empty;


        public string DepartTime { get; set; } = string.Empty;


        public string Destination { get; set; } = string.Empty;


        public string DestinationDecoded { get; set; } = string.Empty;


        public int FlightCount { get; set; } 


        public List<ShopFlight> Flights { get; set; } 


        public string Origin { get; set; } = string.Empty;


        public string OriginDecoded { get; set; } = string.Empty;


        public bool Selected { get; set; } 


        public List<ShopFlattenedFlight> FlattenedFlights { get; set; }

        public ShopTrip()
        {
            Flights = new List<ShopFlight>();
            FlattenedFlights = new List<ShopFlattenedFlight>();
        }
    }
}
