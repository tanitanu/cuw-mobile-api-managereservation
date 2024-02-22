using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBTravelCertificateOrFlightCredit
    {
        private List<MOBETCDetail> travelCreditInfoHeader;
        private List<MOBETCDetail> travelCreditInfoBody;
        private DateTime certficateExpiryDate;
        private string futureFlightCreditLink = string.Empty;
        public List<MOBETCDetail> TravelCreditInfoBody
        {
            get
            {
                return this.travelCreditInfoBody;
            }
            set
            {
                this.travelCreditInfoBody = value;
            }
        }

        public List<MOBETCDetail> TravelCreditInfoHeader
        {
            get
            {
                return this.travelCreditInfoHeader;
            }
            set
            {
                this.travelCreditInfoHeader = value;
            }
        }
        public DateTime CertficateExpiryDate
        {
            get
            {
                return this.certficateExpiryDate;
            }
            set
            {
                this.certficateExpiryDate = value;
            }
        }

        public string FutureFlightCreditLink
        {
            get
            {
                return this.futureFlightCreditLink;
            }
            set
            {
                this.futureFlightCreditLink = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
