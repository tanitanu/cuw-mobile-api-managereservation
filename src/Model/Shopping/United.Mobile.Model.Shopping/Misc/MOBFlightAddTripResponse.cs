using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightAddTripResponse : MOBResponse
    {
        //private FlightAddTripRequest flightAddTripRequest;
        //private FlightAvailability flightAvailability;
        private string selectedTripIndexes = string.Empty;
        private List<FlightAvailabilityTrip> selectedTrips;
        private List<ShopPrice> prices;
        private List<ShopTax> taxes;

        public FlightAddTripResponse()
            : base()
        {
        }

        //public FlightAddTripRequest FlightAddTripRequest
        //{
        //    get
        //    {
        //        return this.flightAddTripRequest;
        //    }
        //    set
        //    {
        //        this.flightAddTripRequest = value;
        //    }
        //}

        //public FlightAvailability FlightAvailability
        //{
        //    get
        //    {
        //        return this.flightAvailability;
        //    }
        //    set
        //    {
        //        this.flightAvailability = value;
        //    }
        //}

        public string SelectedTripIndexes
        {
            get
            {
                return this.selectedTripIndexes;
            }
            set
            {
                this.selectedTripIndexes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<FlightAvailabilityTrip> SelectedTrips
        {
            get { return this.selectedTrips; }
            set { this.selectedTrips = value; }
        }

        public List<ShopPrice> Prices
        {
            get
            {
                return this.prices;
            }
            set
            {
                this.prices = value;
            }
        }

        public List<ShopTax> Taxes
        {
            get
            {
                return this.taxes;
            }
            set
            {
                this.taxes = value;
            }
        }

        private string footerMessage = null;

        public string FooterMessage
        {
            get { return footerMessage; }
            set { footerMessage = value; }
        }
    }
}
