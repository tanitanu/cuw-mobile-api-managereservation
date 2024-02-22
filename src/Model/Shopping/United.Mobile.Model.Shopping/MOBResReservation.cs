using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class MOBResReservation
    {
        private string createDate = string.Empty;
        private List<MOBComEmailAddress> emailAddresses;
        private List<MOBSegReservationFlightSegment> flightSegments;
        private List<MOBComPostalAddress> postalAddresses;
        private List<MOBPriPrice> prices;
        private List<MOBComRemark> remarks;
        private List<MOBComTelephone> telephoneNumbers;
        private List<MOBResTraveler> travelers;
        private bool isRefundable;
        private bool isInternational;
        private bool isFlexibleSegmentExist;
        private long callDuration = 0;

        public string CreateDate
        {
            get
            {
                return this.createDate;
            }
            set
            {
                this.createDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBComEmailAddress> EmailAddresses
        {
            get
            {
                return this.emailAddresses;
            }
            set
            {
                this.emailAddresses = value;
            }
        }

        public List<MOBSegReservationFlightSegment> FlightSegments
        {
            get
            {
                return this.flightSegments;
            }
            set
            {
                this.flightSegments = value;
            }
        }

        public List<MOBComPostalAddress> PostalAddresses
        {
            get
            {
                return this.postalAddresses;
            }
            set
            {
                this.postalAddresses = value;
            }
        }

        public List<MOBPriPrice> Prices
        {
            get
            {
                return this.prices;
            }
            set
            {
                this.prices = value;
            }
        }

        public List<MOBComRemark> Remarks
        {
            get
            {
                return this.remarks;
            }
            set
            {
                this.remarks = value;
            }
        }

        public List<MOBComTelephone> TelephoneNumbers
        {
            get
            {
                return this.telephoneNumbers;
            }
            set
            {
                this.telephoneNumbers = value;
            }
        }

        public List<MOBResTraveler> Travelers
        {
            get
            {
                return this.travelers;
            }
            set
            {
                this.travelers = value;
            }
        }

        public bool IsRefundable
        {
            get
            {
                return this.isRefundable;
            }
            set
            {
                this.isRefundable = value;
            }
        }

        public bool ISInternational
        {
            get
            {
                return this.isInternational;
            }
            set
            {
                this.isInternational = value;
            }
        }

        public bool ISFlexibleSegmentExist
        {
            get
            {
                return this.isFlexibleSegmentExist;
            }
            set
            {
                this.isFlexibleSegmentExist = value;
            }
        }

        public long CallDuration
        {
            get
            {
                return this.callDuration;
            }
            set
            {
                this.callDuration =  value;
            }
        }
    }
}
