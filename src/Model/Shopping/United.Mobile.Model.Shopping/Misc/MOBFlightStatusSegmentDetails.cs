using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FlightStatusSegmentDetails
    {
        private string statusShort = string.Empty; //Delayed/Canceled
        private string statusDescription = string.Empty; //We apolozise about your gate return...
        private string statusTitle = string.Empty; //Delay Details/Cancelation Details
        private string statusSubTitle = string.Empty; //More information about your flight delay/Cancelation
        public string StatusShort
        {
            get
            {
                return statusShort;
            }
            set
            {
                statusShort = value;
            }
        }
        public string StatusDescription
        {
            get
            {
                return statusDescription;
            }
            set
            {
                statusDescription = value;
            }
        }
        public string StatusTitle
        {
            get
            {
                return statusTitle;
            }
            set
            {
                statusTitle = value;
            }
        }
        public string StatusSubTitle
        {
            get
            {
                return statusSubTitle;
            }
            set
            {
                statusSubTitle = value;
            }
        }
    }
}
