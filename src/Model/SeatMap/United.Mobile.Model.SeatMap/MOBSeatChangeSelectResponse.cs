using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Booking;
using MOBSeat = United.Mobile.Model.Shopping.Misc.Seat;


namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBSeatChangeSelectResponse : MOBResponse
    {
        private MOBSeatChangeSelectRequest request;
        private string sessionId = string.Empty;
        private List<MOBSeat> seats;
        private List<Model.Shopping.MOBSeatMap> seatMap;
        private List<MOBTypeOption> exitAdvisory;
        private List<MOBBKTraveler> bookingTravlerInfo;
        private List<Shopping.Booking.MOBBKTrip> selectedTrips;
        private InterLineDeepLink interLineDeepLink;
        private bool isVerticalSeatMapEnabled = false;
       
        private string interlineErrorMessage;

        public string InterlineErrorMessage
        {
            get { return interlineErrorMessage; }
            set { interlineErrorMessage = value; }
        }
        public MOBSeatChangeSelectRequest Request
        {
            get { return this.request; }
            set { this.request = value; }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBSeat> Seats
        {
            get { return this.seats; }
            set { seats = value; }
        }

        public List<Model.Shopping.MOBSeatMap> SeatMap
        {
            get { return this.seatMap; }
            set { this.seatMap = value; }
        }

        public List<MOBTypeOption> ExitAdvisory
        {
            get { return this.exitAdvisory; }
            set { this.exitAdvisory = value; }
        }

        [JsonProperty(PropertyName = "bookingTravlerInfo")]
        [JsonPropertyName("bookingTravlerInfo")]
        public List<MOBBKTraveler> BookingTravelerInfo
        {
            get { return this.bookingTravlerInfo; }
            set { this.bookingTravlerInfo = value; }
        }

        public List<Shopping.Booking.MOBBKTrip> SelectedTrips
        {
            get { return this.selectedTrips; }
            set { this.selectedTrips = value; }
        }

        public InterLineDeepLink InterLineDeepLink
        {
            get { return this.interLineDeepLink; }
            set { this.interLineDeepLink = value; }
        }

        public bool IsVerticalSeatMapEnabled
        {
            get { return isVerticalSeatMapEnabled; }
            set { isVerticalSeatMapEnabled = value; }
        }
    }

    [Serializable()]
    public class InterLineDeepLink
    {
        private bool showInterlineAdvisoryMessage;
        private string interlineAdvisoryMessage;
        private string interlineAdvisoryTitle;
        private string interlineAdvisoryAlertTitle;

        public string InterlineAdvisoryAlertTitle
        {
            get { return interlineAdvisoryAlertTitle; }
            set { interlineAdvisoryAlertTitle = value; }
        }
        public bool ShowInterlineAdvisoryMessage
        {
            get { return showInterlineAdvisoryMessage; }
            set { showInterlineAdvisoryMessage = value; }
        }
        public string InterlineAdvisoryMessage
        {
            get { return interlineAdvisoryMessage; }
            set { interlineAdvisoryMessage = value; }
        }
        public string InterlineAdvisoryTitle
        {
            get { return interlineAdvisoryTitle; }
            set { interlineAdvisoryTitle = value; }
        }

        private string interlineAdvisoryDeepLinkURL;
        public string InterlineAdvisoryDeepLinkURL
        {
            get { return interlineAdvisoryDeepLinkURL; }
            set { interlineAdvisoryDeepLinkURL = value; }
        }     
    }
}

