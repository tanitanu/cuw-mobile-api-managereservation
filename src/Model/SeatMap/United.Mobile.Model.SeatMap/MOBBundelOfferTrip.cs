using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class MOBBundleOfferTrip
    {
        private string originDestination;
        private string tripId;
        private bool isChecked;
        private int price;
        private string tripProductID;
        private List<string> tripProductIDs;


        public string OriginDestination
        {
            get { return originDestination; }
            set { originDestination = value; }
        }

        public string TripId
        {
            get { return tripId; }
            set { tripId = value; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }
        public string TripProductID
        {
            get { return tripProductID; }
            set { tripProductID = value; }
        }
        public List<string> TripProductIDs
        {
            get { return tripProductIDs; }
            set { tripProductIDs = value; }
        }
    }
}
