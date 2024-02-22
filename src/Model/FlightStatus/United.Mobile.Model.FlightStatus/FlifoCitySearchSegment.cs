using System;
using System.Collections.Generic;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoCitySearchSegment
    {
        private string originName = string.Empty;
        private string destinationName = string.Empty;

        public List<FlifoCitySearchSegment> Legs { get; set; } 

        public string Leg { get; set; } = string.Empty;

        public string Stops { get; set; } = string.Empty;

        public string Origin { get; set; } = string.Empty;

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

        public string Destination { get; set; } = string.Empty;

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

        public string FlightNumber { get; set; } = string.Empty;

        public string DisplayFlightNumber { get; set; } = string.Empty;

        public string DepartureDate { get; set; } = string.Empty;

        public string DepartureTime { get; set; } = string.Empty;

        public string DepartureDateTime { get; set; } = string.Empty;


        public string ArrivalDateTime { get; set; } = string.Empty;

        public string ArrivalOffset { get; set; } = string.Empty;

        public string Equipment { get; set; } = string.Empty;

        public string FcMeal { get; set; } = string.Empty;

        public string EcMeal { get; set; } = string.Empty;

        public string MealTemp { get; set; } = string.Empty;

        public string ClassOfService { get; set; } = string.Empty;

        public string EliteQualification { get; set; } = string.Empty;

        public string OnTimePercentage { get; set; } = string.Empty;

        public string Miles { get; set; } = string.Empty;

        public string TravelTime { get; set; } = string.Empty;

        public string TravelTimeSort { get; set; } = string.Empty;

        public string SVCMap { get; set; } = string.Empty;

        public string OperatedBy { get; set; } = string.Empty;

        public string OperatedByCode { get; set; } = string.Empty;

        public string MarketedBy { get; set; } = string.Empty;

        public string FlifoStatusMessage { get; set; } = string.Empty;

        public string FlightStatusMessage { get; set; } = string.Empty;

        public string FlightStatusTimeStamp { get; set; } = string.Empty;
        public string CodeShareFlightNumber { get; set; } = string.Empty;
        public string DepartureTimeSort { get; set; } = string.Empty;

    }
}