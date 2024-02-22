using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKReward
    {
        private string tripId;
        private string flightId;
        private string rewardId;
        private bool available;
        private string cabin = string.Empty;
        private decimal changeFee;
        private decimal changeFeeTotal;
        private string crossCabinMessaging = string.Empty;
        private string fareBasis = string.Empty;
        private decimal mileage;
        private decimal mileageCollect;
        private decimal mileageCollectTotal;
        private decimal mileageTotal;
        private bool promotion;
        private string rewardCode = string.Empty;
        private string rewardType = string.Empty;
        private bool selected;
        private string status = string.Empty;
        private List<MOBBKTax> taxes;
        private decimal taxTotal;
        private decimal taxAndFeeTotal;
        private List<string> descriptions;

        public string TripId
        {
            get
            {
                return this.tripId;
            }
            set
            {
                this.tripId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightId
        {
            get
            {
                return this.flightId;
            }
            set
            {
                this.flightId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RewardId
        {
            get
            {
                return this.rewardId;
            }
            set
            {
                this.rewardId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool Available
        {
            get
            {
                return this.available;
            }
            set
            {
                this.available = value;
            }
        }

        public string Cabin
        {
            get
            {
                return this.cabin;
            }
            set
            {
                this.cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public decimal ChangeFee
        {
            get
            {
                return this.changeFee;
            }
            set
            {
                this.changeFee = value;
            }
        }

        public decimal ChangeFeeTotal
        {
            get
            {
                return this.changeFeeTotal;
            }
            set
            {
                this.changeFeeTotal = value;
            }
        }

        public string CrossCabinMessaging
        {
            get
            {
                return this.crossCabinMessaging;
            }
            set
            {
                this.crossCabinMessaging = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FareBasis
        {
            get
            {
                return this.fareBasis;
            }
            set
            {
                this.fareBasis = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public decimal Mileage
        {
            get
            {
                return this.mileage;
            }
            set
            {
                this.mileage = value;
            }
        }

        public decimal MileageCollect
        {
            get
            {
                return this.mileageCollect;
            }
            set
            {
                this.mileageCollect = value;
            }
        }

        public decimal MileageCollectTotal
        {
            get
            {
                return this.mileageCollectTotal;
            }
            set
            {
                this.mileageCollectTotal = value;
            }
        }

        public decimal MileageTotal
        {
            get
            {
                return this.mileageTotal;
            }
            set
            {
                this.mileageTotal = value;
            }
        }

        public string RewardCode
        {
            get
            {
                return this.rewardCode;
            }
            set
            {
                this.rewardCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string RewardType
        {
            get
            {
                return this.rewardType;
            }
            set
            {
                this.rewardType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool Selected
        {
            get
            {
                return this.selected;
            }
            set
            {
                this.selected = value;
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBBKTax> Taxes
        {
            get
            {
                return this.taxes;
            }
            set
            {
                this.taxes = value;
            }
        }

        public decimal TaxTotal
        {
            get
            {
                return this.taxTotal;
            }
            set
            {
                this.taxTotal = value;
            }
        }

        public decimal TaxAndFeeTotal
        {
            get
            {
                return this.taxAndFeeTotal;
            }
            set
            {
                this.taxAndFeeTotal = value;
            }
        }

        public List<string> Descriptions
        {
            get
            {
                return this.descriptions;
            }
            set
            {
                this.descriptions = value;
            }
        }
    }
}
