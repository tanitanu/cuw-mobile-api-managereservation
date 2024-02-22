using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.UnfinishedBooking
{
    [Serializable()]
    public class MOBGetOmniCartSavedTripsResponse : MOBResponse
    {
        private List<MOBOmniCartSavedTrip> omniCartSavedTrips;

        public List<MOBOmniCartSavedTrip> OmniCartSavedTrips
        {
            get { return omniCartSavedTrips; }
            set { omniCartSavedTrips = value; }
        }
        private string header;

        public string Header
        {
            get { return header; }
            set { header = value; }
        }
        private string footer;

        public string Footer
        {
            get { return footer; }
            set { footer = value; }
        }
        private string cartTitle;

        public string CartTitle
        {
            get { return cartTitle; }
            set { cartTitle = value; }
        }
        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
        private List<MOBLink> links;

        public List<MOBLink> Links
        {
            get { return links; }
            set { links = value; }
        }
        private MOBSection warningMessages;

        public MOBSection WarningMessages
        {
            get { return warningMessages; }
            set { warningMessages = value; }
        }
        private bool showOmniCartIndicator;

        public bool ShowOmniCartIndicator
        {
            get { return showOmniCartIndicator; }
            set { showOmniCartIndicator = value; }
        }
    }
}
