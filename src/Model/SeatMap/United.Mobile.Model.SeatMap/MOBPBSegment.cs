using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class MOBPBSegment
    {
        private string origin = string.Empty;
        private string destination = string.Empty;
        private string flightDate = string.Empty;
        private List<MOBPBCustomer> customers = null;
        private MOBPBSegmentType pbSegmentType;
        private string segmentId = string.Empty;
        private string message = string.Empty;
        private int fee; // new change. PB will be offered based on Segment instead of based on traveler. Might change later 
        private bool isChecked; // client usage
        private int custPrice; // for build segment info as $54/traveler
        private List<string> offerIds; //["S10", "S11"]

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public string FlightDate
        {
            get { return flightDate; }
            set { flightDate = value; }
        }

        public List<MOBPBCustomer> Customers
        {
            get { return customers; }
            set { customers = value; }
        }

        public MOBPBSegmentType PbSegmentType
        {
            get { return pbSegmentType; }
            set { pbSegmentType = value; }
        }

        public string SegmentId
        {
            get { return segmentId; }
            set { segmentId = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public int Fee
        {
            get { return fee; }
            set { fee = value; }
        }

        public int CustPrice
        {
            get { return custPrice; }
            set { custPrice = value; }
        }

        public List<string> OfferIds
        {
            get { return offerIds; }
            set { offerIds = value; }
        }
    }
}


