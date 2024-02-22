using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBTravelType
    {
        private string travelType;
        private string travelDescription;

        public string TravelType
        {
            get { return this.travelType; }
            set { this.travelType = value; }
        }

        public string TravelDescription
        {
            get { return this.travelDescription; }
            set { this.travelDescription = value; }
        }
    }
}
