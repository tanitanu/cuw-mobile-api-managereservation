using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.ManageRes
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
