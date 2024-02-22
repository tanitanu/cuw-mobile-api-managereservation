using System;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable]
    public class PNRSegment : Segment
    {
        private string recordLocator = string.Empty;
        private string seats = string.Empty;
        private string activationDateTimeGMT = string.Empty;
        private string lastUpdated = string.Empty;
        private string cabin = string.Empty;
        private string cabinType = string.Empty;
        private string classOfService = string.Empty;
        private string tripNumber = string.Empty;
        private string segmentNumber = string.Empty;

        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Seats
        {
            get
            {
                return this.seats;
            }
            set
            {
                this.seats = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ActivationDateTimeGMT
        {
            get
            {
                return this.activationDateTimeGMT;
            }
            set
            {
                this.activationDateTimeGMT = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastUpdated
        {
            get
            {
                return this.lastUpdated;
            }
            set
            {
                this.lastUpdated = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string CabinType
        {
            get
            {
                return this.cabinType;
            }
            set
            {
                this.cabinType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                this.Cabin = GetCabin(this.cabinType);
            }
        }

        public string ClassOfService
        {
            get
            {
                return this.classOfService;
            }
            set
            {
                this.classOfService = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TripNumber
        {
            get { return tripNumber; }
            set { tripNumber = value; }
        }

        public string SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }


        private string GetCabin(string cabinType)
        {
            string cabin = string.Empty;

            if (!string.IsNullOrEmpty(cabinType))
            {
                switch (cabinType.ToLower())
                {
                    case "united economy":
                        cabin = "Coach";
                        break;
                    case "economy":
                        cabin = "Coach";
                        break;
                    case "united business":
                        cabin = "Business";
                        break;
                    case "business":
                        cabin = "Business";
                        break;
                    case "united businessfirst":
                        cabin = "BusinessFirst";
                        break;
                    case "businessfirst":
                        cabin = "BusinessFirst";
                        break;
                    case "united global first":
                        cabin = "First";
                        break;
                    case "united first":
                        cabin = "First";
                        break;
                    case "first":
                        cabin = "First";
                        break;
                }
            }
            return cabin;
        }
    }
}
