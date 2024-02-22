using System;
using System.Collections.Generic;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoCitySearchTrip
    {
        private string originName = string.Empty;
        private string destinationName = string.Empty;

        public List<FlifoCitySearchSegment> ScheduleSegments { get; set; } = new List<FlifoCitySearchSegment>();

        public int Index { get; set; }

        public string Origin { get; set; }

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

        public string Destination { get; set; }

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

        public string Stops { get; set; }

        public string JourneyTime { get; set; }

        public string GroundTime { get; set; }
        public string JourneyMileage { get; set; } = string.Empty;

        public string DepartureDateTime { get; set; }
    }
}