using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Services.FlightShopping.Common;
using ShopResponse = United.Services.FlightShopping.Common.ShopResponse;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable()]
   public class MOBTripPlanSummaryResponse:MOBResponse
    {
        
        public bool ShouldShowDeleteTrip { get; set; }


        public MOBTripPlanOnboarding TripPlanOnboarding { get; set; }

        public List<InfoWarningMessages> InfoWarningMessages { get; set; }

        public string SessionId { get; set; }

        public MOBTripPlanSummaryRequest Request { get; set; }
        

        public string TripPlanId { get; set; }
        

        public List<MOBTripPlanTrip> TripPlanTrips { get; set; }        

        public List<MOBItem> Captions { get; set; }
       

        public List<MOBTravelerType> TravelerTypes { get; set; }    

        public string TripPlannerType { get; set; }       

        public string TripPlanShareLink { get; set; }
       

    }

    [Serializable()]
    public class MOBTripPlanTrip
    {
        private string cartId = string.Empty;
        private string itineraryTitle;
        private string itineraryPriceText;
        private int numberOfVotes;
        private string bookingText;
        private string voteText; 
        private bool hideVotes;
        private int numberOfBookings;
        private List<MOBTripPlanLOF> tripPlanLOFs;
        private string firstFlightDepartDate;
        private MOBTripVoteInfo voteInfo;
        private string editText;
        private string addTripBtnText;
        private string itineraryUnavailableText;
        private string pnrDetailsLinkText;
        private bool isNotAvailable;
        private string bookLinkText;

        public bool HideVotes { get; set; }

        private List<MOBStyledText> badges;
        public List<MOBStyledText> Badges
        {
            get { return badges; }
            set { badges = value; }
        }

        public string ItineraryUnavailableText
        {
            get { return itineraryUnavailableText; }
            set { itineraryUnavailableText = value; }
        }

        public string BookLinkText
        {
            get { return bookLinkText; }
            set { bookLinkText = value; }
        }
        public bool IsNotAvailable
        {
            get { return isNotAvailable; }
            set { isNotAvailable = value; }
        }
        public string BookingText
        {
            get { return bookingText; }
            set { bookingText = value; }
        }
        public string VoteText
        {
            get { return voteText; }
            set { voteText = value; }
        }
        public string PnrDetailsLinkText
        {
            get { return pnrDetailsLinkText; }
            set { pnrDetailsLinkText = value; }
        }
        public string AddTripBtnText
        {
            get { return addTripBtnText; }
            set { addTripBtnText = value; }
        }
        public string EditText
        {
            get { return editText; }
            set { editText = value; }
        }
        public string CartId
        {
            get
            {
                return cartId;
            }
            set
            {
                cartId = value;
            }
        }

        public string FirstFlightDepartDate
        {
            get
            {
                return firstFlightDepartDate;
            }
            set
            {
                firstFlightDepartDate = value;
            }
        }

        public string ItineraryTitle
        {
            get { return itineraryTitle; }
            set { itineraryTitle = value; }
        }

        public string ItineraryPriceText
        {
            get { return itineraryPriceText; }
            set { itineraryPriceText = value; }
        }

        public int NumberOfVotes
        {
            get { return numberOfVotes; }
            set { numberOfVotes = value; }
        }

        public int NumberOfBookings
        {
            get { return numberOfBookings; }
            set { numberOfBookings = value; }
        }
      
        public List<MOBTripPlanLOF> TripPlanLOFs
        {
            get { return tripPlanLOFs; }
            set { tripPlanLOFs = value; }
        }

        public MOBTripVoteInfo VoteInfo
        {
            get
            {
                return voteInfo;
            }
            set
            {
                voteInfo = value;
            }
        }
    }
    [Serializable()]
    public class MOBTripVoteInfo
    {
        private bool isVoted;
        private string voteId;

        public bool IsVoted
        {
            get { return isVoted; }
            set { isVoted = value; }
        }

        public string VoteId
        {
            get { return voteId; }
            set { voteId = value; }
        }
    }

    [Serializable()]
    public class MOBTripPlanLOF
    {
        private string title;
        private string subTitle;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }

    }
    //Delete MOBTripPlanCCEMemberData once CCE shares updated Models for tripplaner
    public class MOBTripPlanCCEMemberData
    {
        //Type=Tuple<string,ShopRequest,ShopResponse>
        private string cartId;
        private ShopRequest shopRequest;
        private ShopResponse shopResponse;

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }
        public ShopResponse ShopResponse
        {
            get { return shopResponse; }
            set { shopResponse = value; }
        }

        public ShopRequest ShopRequest
        {
            get { return shopRequest; }
            set { shopRequest = value; }
        }


    }
}
