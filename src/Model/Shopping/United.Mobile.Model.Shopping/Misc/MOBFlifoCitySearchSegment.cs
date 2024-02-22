using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoCitySearchSegment
    {
        private string leg = string.Empty;
        private string stops = string.Empty;
        private string origin = string.Empty;
        private string originName = string.Empty;
        private string destination = string.Empty;
        private string destinationName = string.Empty;
        private string flightNumber = string.Empty;
        private string displayFlightNumber = string.Empty;
        private string codeShareFlightNumber = string.Empty;
        private string departureDate = string.Empty;
        private string departureTime = string.Empty;
        private string departureDateTime = string.Empty;
        private string departureTimeSort = string.Empty;
        private string arrivalDateTime = string.Empty;
        private string arrivalOffset = string.Empty;
        private string equipment = string.Empty;
        private string mealTemp = string.Empty;
        private string classOfService = string.Empty;
        private string fcMeal = string.Empty;
        private string ecMeal = string.Empty;
        private string miles = string.Empty;
        private string travelTime = string.Empty;
        private string travelTimeSort = string.Empty;
        private string eliteQualification = string.Empty;
        private string onTimePercentage = string.Empty;
        private string svcMap = string.Empty;
        private string operatedBy = string.Empty;
        private string operatedByCode = string.Empty;
        private string marketedBy = string.Empty;
        private List<FlifoCitySearchSegment> legs;
        private string flifoStatusMessage = string.Empty;
        private string flightStatusMessage = string.Empty;
        private string flightStatusTimeStamp = string.Empty;

        public List<FlifoCitySearchSegment> Legs
        {
            get { return legs; }
            set { legs = value; }
        }

        public string Leg
        {
            get
            {
                return leg;
            }
            set
            {
                this.leg = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Stops
        {
            get
            {
                return stops;
            }
            set
            {
                this.stops = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Origin
        {
            get
            {
                return origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string OriginName
        {
            get
            {
                return originName;
            }
            set
            {
                this.originName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                if (this.originName.IndexOf('-') != -1)
                {
                    int pos = this.originName.IndexOf('-');
                    this.originName = string.Format("{0})", this.originName.Substring(0, pos).Trim());
                }

            }
        }

        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DestinationName
        {
            get
            {
                return destinationName;
            }
            set
            {
                this.destinationName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                if (this.destinationName.IndexOf('-') != -1)
                {
                    int pos = this.destinationName.IndexOf('-');
                    this.destinationName = string.Format("{0})", this.destinationName.Substring(0, pos).Trim());
                }
            }
        }

        public string FlightNumber
        {
            get
            {
                return flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DisplayFlightNumber
        {
            get
            {
                return displayFlightNumber;
            }
            set
            {
                this.displayFlightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureDate
        {
            get
            {
                return departureDate;
            }
            set
            {
                this.departureDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureTime
        {
            get
            {
                return departureTime;
            }
            set
            {
                this.departureTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DepartureDateTime
        {
            get
            {
                return departureDateTime;
            }
            set
            {
                this.departureDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        

        public string ArrivalDateTime
        {
            get
            {
                return this.arrivalDateTime;
            }
            set
            {
                this.arrivalDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ArrivalOffset
        {
            get
            {
                return this.arrivalOffset;
            }
            set
            {
                this.arrivalOffset = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Equipment
        {
            get
            {
                return this.equipment;
            }
            set
            {
                this.equipment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FcMeal
        {
            get
            {
                return this.fcMeal;
            }
            set
            {
                this.fcMeal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EcMeal
        {
            get
            {
                return this.ecMeal;
            }
            set
            {
                this.ecMeal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MealTemp
        {
            get
            {
                return this.mealTemp;
            }
            set
            {
                this.mealTemp = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string EliteQualification
        {
            get
            {
                return this.eliteQualification;
            }
            set
            {
                this.eliteQualification = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OnTimePercentage
        {
            get
            {
                return this.onTimePercentage;
            }
            set
            {
                this.onTimePercentage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public string Miles
        {
            get
            {
                return this.miles;
            }
            set
            {
                this.miles = string.IsNullOrEmpty(value) ? string.Empty : value.TrimStart('0');
            }
        }


        public string TravelTime
        {
            get
            {
                return this.travelTime;
            }
            set
            {
                this.travelTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TravelTimeSort
        {
            get
            {
                return this.travelTimeSort;
            }
            set
            {
                this.travelTimeSort = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SVCMap
        {
            get
            {
                return this.svcMap;
            }
            set
            {
                this.svcMap = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }


        public string OperatedBy
        {
            get
            {
                return this.operatedBy;
            }
            set
            {
                this.operatedBy = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string OperatedByCode
        {
            get
            {
                return this.operatedByCode;
            }
            set
            {
                this.operatedByCode = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string MarketedBy
        {
            get
            {
                return this.marketedBy;
            }
            set
            {
                this.marketedBy = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string FlifoStatusMessage
        {
            get
            {
                return this.flifoStatusMessage;
            }
            set
            {
                this.flifoStatusMessage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string FlightStatusMessage
        {
            get
            {
                return this.flightStatusMessage;
            }
            set
            {
                this.flightStatusMessage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string FlightStatusTimeStamp
        {
            get
            {
                return this.flightStatusTimeStamp;
            }
            set
            {
                this.flightStatusTimeStamp = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
    }
}
