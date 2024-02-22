using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.MPRewards
{
    public class SeatEngine
    {
        #region Properties
        //private string pnrCreationDate = string.Empty;
        //private List<MOBBKTraveler> bookingTravelerInfo;
        //private List<MOBBKTrip> selectedTrips;
        //private List<TripSegment> segments;
        //private List<Seat> seats;
        //private int numberOfEPAMembersInthePNR = 0;
        //private string pcuOfferAmountForthisCabin;
        //private string cabinName = string.Empty;
        //private string pcuOfferOptionId;
        //private double pcuOfferPriceForthisCabin = 0;
        //private string recordLocator;
        //private string lastName;
        //private string pointOfSale;

        public string PointOfSale { get; set; }
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public string PNRCreationDate { get; set; }
        public List<MOBBKTraveler> BookingTravelerInfo { get; set; }
        public List<Shopping.Booking.MOBBKTrip> SelectedTrips { get; set; }
        public List<TripSegment> Segments { get; set; }
        public List<Seat> Seats { get; set; }
        public int NumberOfEPAMembersInthePNR { get; set; }
        public bool IsVerticalSeatMapEnabled { get; set; }


        public SeatEngine()
        {
            BookingTravelerInfo = new List<MOBBKTraveler>();
            SelectedTrips = new List<Shopping.Booking.MOBBKTrip>();
            Segments = new List<TripSegment>();
            Seats = new List<Seat>();
        }
        #endregion of properties
    }
}
