using System;
using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class MOBBundleDetail
    {
        private string offerTitle;
        private string offerWarningMessage;
        private List<MOBBundleOfferDetail> offerDetails;
        private List<MOBBundleOfferTrip> offerTrips;


        public string OfferTitle
        {
            get { return offerTitle; }
            set { offerTitle = value; }
        }
        public string OfferWarningMessage
        {
            get { return offerWarningMessage; }
            set { offerWarningMessage = value; }
        }

        public List<MOBBundleOfferDetail> OfferDetails
        {
            get { return offerDetails; }
            set { offerDetails = value; }
        }

        public List<MOBBundleOfferTrip> OfferTrips
        {
            get { return offerTrips; }
            set { offerTrips = value; }
        }


    }
}
