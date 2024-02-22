using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoCitySearchTrip
    {
        private int index;
        private string origin = string.Empty;
        private string originName = string.Empty;
        private string destination = string.Empty;
        private string destinationName = string.Empty;
        private string stops = string.Empty;
        private string journeyTime = string.Empty;
        private string groundTime = string.Empty;
        private string journeyMileage = string.Empty;
        private string departureDateTime = string.Empty;

        private List<FlifoCitySearchSegment> scheduleSegments;


        public List<FlifoCitySearchSegment> ScheduleSegments
        {
            get
            {
                return this.scheduleSegments;
            }
            set
            {
                this.scheduleSegments = value;
            }
        }


        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                this.index = value;
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
                return this.destination;
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

        public string Stops
        {
            get
            {
                return this.stops;
            }
            set
            {
                this.stops = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string JourneyTime
        {
            get
            {
                return this.journeyTime;
            }
            set
            {
                this.journeyTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToLower();
            }
        }

        public string GroundTime
        {
            get
            {
                return this.groundTime;
            }
            set
            {
                this.groundTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToLower();
            }
        }

        public string DepartureDateTime
        {
            get
            {
                return this.departureDateTime;
            }
            set
            {
                this.departureDateTime = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        

        

    }
}
