using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoScheduleTrip
    {
        public string DepartureDate = string.Empty;

        public string DepartureTime = string.Empty;

        public string Destination = string.Empty;

        public FlifoScheduleFlight[] Flights;

        public string GroundTime = string.Empty;

        public string JourneyTime = string.Empty;

        public string OperationDays = string.Empty;

        public string Origin = string.Empty;

        public string Stops = string.Empty;

        public string TripNumber= string.Empty;

        ///// <remarks/>
        //public string DepartureDate
        //{
        //    get
        //    {
        //        return this.departureDateField;
        //    }
        //    set
        //    {
        //        this.departureDateField = value;
        //    }
        //}

        ///// <remarks/>
        //public string DepartureTime
        //{
        //    get
        //    {
        //        return this.departureTimeField;
        //    }
        //    set
        //    {
        //        this.departureTimeField = value;
        //    }
        //}

        ///// <remarks/>
        //public string Destination
        //{
        //    get
        //    {
        //        return this.destinationField;
        //    }
        //    set
        //    {
        //        this.destinationField = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlArrayItemAttribute("Flight", IsNullable = false)]
        //public FlifoScheduleTripFlight[] Flights
        //{
        //    get
        //    {
        //        return this.flightsField;
        //    }
        //    set
        //    {
        //        this.flightsField = value;
        //    }
        //}

        ///// <remarks/>
        //public string GroundTime
        //{
        //    get
        //    {
        //        return this.groundTimeField;
        //    }
        //    set
        //    {
        //        this.groundTimeField = value;
        //    }
        //}

        ///// <remarks/>
        //public string JourneyTime
        //{
        //    get
        //    {
        //        return this.journeyTimeField;
        //    }
        //    set
        //    {
        //        this.journeyTimeField = value;
        //    }
        //}

        ///// <remarks/>
        //public string OperationDays
        //{
        //    get
        //    {
        //        return this.operationDaysField;
        //    }
        //    set
        //    {
        //        this.operationDaysField = value;
        //    }
        //}

        ///// <remarks/>
        //public string Origin
        //{
        //    get
        //    {
        //        return this.originField;
        //    }
        //    set
        //    {
        //        this.originField = value;
        //    }
        //}

        ///// <remarks/>
        //public string Stops
        //{
        //    get
        //    {
        //        return this.stopsField;
        //    }
        //    set
        //    {
        //        this.stopsField = value;
        //    }
        //}

        ///// <remarks/>
        //public string TripNumber
        //{
        //    get
        //    {
        //        return this.tripNumberField;
        //    }
        //    set
        //    {
        //        this.tripNumberField = value;
        //    }
        //}
    }
}
