using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class TravelerPricing
    {
        private int travelerId;
        public int TravelerId
        {
            get { return this.travelerId; }
            set { this.travelerId = value; }
        }

        private string displayFeesWithMiles;
        public string DisplayFeesWithMiles
        {
            get { return this.displayFeesWithMiles; }
            set { this.displayFeesWithMiles = value; }
        }

        private string priceDifferenceLabel;
        public string PriceDifferenceLabel
        {
            get { return this.priceDifferenceLabel; }
            set { this.priceDifferenceLabel = value; }
        }

    }
}
