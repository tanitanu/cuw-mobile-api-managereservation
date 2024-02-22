using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKMakeReservationResponse : MOBResponse
    {
        private string sessionID = string.Empty;
        private MOBBKReservation reservation;
        private List<string> dotBagRules;
        private DOTBaggageInfo dotBaggageInfo;
        private string shareFlightTitle = string.Empty;
        private string shareFlightMessage = string.Empty;
        private string warning = string.Empty;
        private string fqtvNameMismatchMessage = string.Empty; 
        private List<string> disclaimer ;

        public string SessionID
        {
            get
            {
                return this.sessionID;
            }
            set
            {
                this.sessionID = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBBKReservation Reservation
        {
            get
            {
                return this.reservation;
            }
            set
            {
                this.reservation = value;
            }
        }

        public List<string> DOTBagRules
        {
            get
            {
                return this.dotBagRules;
            }
            set
            {
                this.dotBagRules = value;
            }
        }

        public DOTBaggageInfo DOTBaggageInfo
        {
            get
            {
                return this.dotBaggageInfo;
            }
            set
            {
                this.dotBaggageInfo = value;
            }
        }

        public string ShareFlightTitle
        {
            get
            {
                return this.shareFlightTitle;
            }
            set
            {
                this.shareFlightTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ShareFlightMessage
        {
            get
            {
                return this.shareFlightMessage;
            }
            set
            {
                this.shareFlightMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Warning
        {
            get
            {
                return this.warning;
            }
            set
            {
                this.warning = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FQTVNameMismatchMessage
        {
            get
            {
                return this.fqtvNameMismatchMessage;
            }
            set
            {
                this.fqtvNameMismatchMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                this.disclaimer = value;
            }
        }
    }
}
