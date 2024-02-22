using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BundleInfo
    {
        private List<BundleFlightSegment> flightSegments;
        //private List<BundleTraveler> travelers;
        private string errorMessage = string.Empty;

        public List<BundleFlightSegment> FlightSegments
        {
            get { return this.flightSegments; }
            set { this.flightSegments = value; }
        }

        //public List<BundleTraveler> Travelers
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
