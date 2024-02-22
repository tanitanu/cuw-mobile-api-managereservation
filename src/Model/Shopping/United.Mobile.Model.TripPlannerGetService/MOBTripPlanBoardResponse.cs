using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable()]
    public class MOBTripPlanBoardResponse:MOBResponse
    {
        private MOBTripPlanBoardRequest request;
        private List<MOBItem> captions; // Title, body text & Button text 
        private List<TripPlanShareTrips> groupedTrips;
        private List<InfoWarningMessages> warningMessages;

        public MOBTripPlanBoardRequest Request
        {
            get { return request; }
            set { request = value; }
        }

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }
       
        public List<TripPlanShareTrips> GroupedTrips
        {
            get { return groupedTrips; }
            set { groupedTrips = value; }
        }

        public List<InfoWarningMessages> WarningMessages
        {
            get { return warningMessages; }
            set { warningMessages = value; }
        }
    }
    [Serializable()]
    public class TripPlanShareTrips
    {
        private string shareTitle;
        private string viewMoreTripsText;
        private string viewLessTripsText;
        private string noTripsMessage;
        private int noOfTripsToShow;
        private List<TripPlanDetail> tripPlanDetails;

        public int NoOfTripsToShow
        {
            get { return noOfTripsToShow; }
            set { noOfTripsToShow = value; }
        }

        public string NoTripsMessage
        {
            get { return noTripsMessage; }
            set { noTripsMessage = value; }
        }


        public string ViewLessTripsText
        {
            get { return viewLessTripsText; }
            set { viewLessTripsText = value; }
        }


        public string ViewMoreTripsText
        {
            get { return viewMoreTripsText; }
            set { viewMoreTripsText = value; }
        }


        public string ShareTitle
        {
            get { return shareTitle; }
            set { shareTitle = value; }
        }

        public List<TripPlanDetail> TripPlanDetails
        {
            get { return tripPlanDetails; }
            set { tripPlanDetails = value; }
        }
    }
    [Serializable()]
    public class TripPlanDetail
    {
        private string title;
        private string subTitle;
        private string infoMessage;
        private string tripPlanId;

        public string TripPlanId
        {
            get { return tripPlanId; }
            set { tripPlanId = value; }
        }

        public string InfoMessage
        {
            get { return infoMessage; }
            set { infoMessage = value; }
        }

        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

    }
}
