using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBBundleInfo
    {
        private List<MOBBundleFlightSegment> flightSegments;
        //private List<MOBBundleTraveler> travelers;
        private string errorMessage = string.Empty;

        public List<MOBBundleFlightSegment> FlightSegments
        {
            get { return this.flightSegments; }
            set { this.flightSegments = value; }
        }

        //public List<MOBBundleTraveler> Travelers
        //{
        //    get { return this.travelers; }
        //    set { this.travelers = value; }
        //}

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
