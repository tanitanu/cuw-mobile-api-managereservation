using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.UnfinishedBooking
{
    [Serializable()]
    public class MOBOmniCartSavedTrip
    {
        private List<MOBOmniCartSavedTripDetails> trips;

        public List<MOBOmniCartSavedTripDetails> Trips
        {
            get { return trips; }
            set { trips = value; }
        }

        private List<MOBLink> links;
        public List<MOBLink> Links
        {
            get { return links; }
            set { links = value; }
        }
        private string cartId;

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        private MOBOmniCartSavedTripDetails tripInfo;

        public MOBOmniCartSavedTripDetails TripInfo
        {
            get { return tripInfo; }
            set { tripInfo = value; }
        }
    }
    [Serializable()]
    public class MOBOmniCartSavedTripDetails
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string subTitle;
        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }
        private string subTitle2;

        public string SubTitle2
        {
            get { return subTitle2; }
            set { subTitle2 = value; }
        }

        private List<MOBOmniCartSavedTripDetails> flightDetails;

        public List<MOBOmniCartSavedTripDetails> FlightDetails
        {
            get { return flightDetails; }
            set { flightDetails = value; }
        }
        private string connectionTime;

        public string ConnectionTime
        {
            get { return connectionTime; }
            set { connectionTime = value; }
        }
        private List<string> selectedAncillaries;

        public List<string> SelectedAncillaries
        {
            get { return selectedAncillaries; }
            set { selectedAncillaries = value; }
        }
    }
    [Serializable()]
    public class MOBLink
    {
        private string componentTitle;

        public string ComponentTitle
        {
            get { return componentTitle; }
            set { componentTitle = value; }
        }
        private string linkText;

        public string LinkText
        {
            get { return linkText; }
            set { linkText = value; }
        }

    }
}
