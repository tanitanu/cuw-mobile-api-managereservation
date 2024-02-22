using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.ShopSeats
{
    [Serializable()]
    [XmlRoot("MOBSeatMapCSL30")]
    public class MOBSeatMapCSL30
    {
        public string ObjectName {get;set;} = "United.Definition.SeatCSL30.MOBSeatMapCSL30";
        private int flightNumber;
        private string flightDateTime = string.Empty;
        private string departureCode;
        private string arrivalCode;
        private bool isOaSeatMap;
        private string marketingCarrierCode = string.Empty;
        private string operatingCarrierCode = string.Empty;
        private string flow = string.Empty;
        private int segmentNumber;
        private List<MOBSeatCSL30> seat;
   
        public int FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = value;
            }
        }

        public string FlightDateTime
        {
            get
            {
                return this.flightDateTime;
            }
            set
            {
                this.flightDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureCode
        {
            get
            {
                return this.departureCode;
            }
            set
            {
                this.departureCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ArrivalCode
        {
            get
            {
                return this.arrivalCode;
            }
            set
            {
                this.arrivalCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsOaSeatMap
        {
            get { return isOaSeatMap; }
            set { isOaSeatMap = value; }
        }

        public string MarketingCarrierCode
        {
            get
            {
                return this.marketingCarrierCode;
            }
            set
            {
                this.marketingCarrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OperatingCarrierCode
        {
            get
            {
                return this.operatingCarrierCode;
            }
            set
            {
                this.operatingCarrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int SegmentNumber
        {
            get
            {
                return this.segmentNumber;
            }
            set
            {
                this.segmentNumber = value;
            }
        }

        [XmlArrayItem("MOBSeatCSL30")]
        public List<MOBSeatCSL30> Seat
        {
            get { return seat; }
            set { seat = value; }
        }

    }

    [Serializable()]
    public class MOBSeatCSL30
    {
        public string number;
        public string tier;
        public string eDoc;
        public string seatType;
        public bool isAvailable;
        public decimal totalFee;
        public string displaySeatCategory;
        public List<TierPricingCSL30> pricing;

        public string Number
        {
            get
            {
                return this.number;
            }
            set
            {
                this.number = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Tier
        {
            get
            {
                return this.tier;
            }
            set
            {
                this.tier = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EDoc
        {
            get
            {
                return this.eDoc;
            }
            set
            {
                this.eDoc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SeatType
        {
            get
            {
                return this.seatType;
            }
            set
            {
                this.seatType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DisplaySeatCategory
        {
            get
            {
                return this.displaySeatCategory;
            }
            set
            {
                this.displaySeatCategory = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; }
        }

        public decimal TotalFee
        {
            get
            {
                return this.totalFee;
            }
            set
            {
                this.totalFee = value;
            }
        }

        [XmlArrayItem("MOBTierPricingCSL30")]
        public List<TierPricingCSL30> Pricing
        {
            get { return pricing; }
            set { pricing = value; }
        }
    }

    [Serializable()]
    public class TierPricingCSL30
    {
        public int travelerId;
        private string travelerIndex;
        public decimal totalPrice;
        private string originalPrice;
        private string couponCode { get; set; }
        private string currencyCode { get; set; }
        
        public int TravelerId
        {
            get
            {
                return this.travelerId;
            }
            set
            {
                this.travelerId = value;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return this.totalPrice;
            }
            set
            {
                this.totalPrice = value;
            }
        }

        public string TravelerIndex
        {
            get
            {
                return this.travelerIndex;
            }
            set
            {
                this.travelerIndex = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string OriginalPrice
        {
            get
            {
                return this.originalPrice;
            }
            set
            {
                this.originalPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CouponCode
        {
            get
            {
                return this.couponCode;
            }
            set
            {
                this.couponCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CurrencyCode
        {
            get
            {
                return this.currencyCode;
            }
            set
            {
                this.currencyCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
