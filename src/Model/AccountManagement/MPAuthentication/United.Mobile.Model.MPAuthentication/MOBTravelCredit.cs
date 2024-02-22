using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBTravelCredit
    {
        private string travelCreditAvailableText = string.Empty;
        private string travelCreditPageTitle = string.Empty;
        private string travelCreditHeaderTitle = string.Empty;
        private string travelCreditHeaderBody = string.Empty;
        private List<MOBTravelCertificateOrFlightCredit> travelCertificateOrFlightCreditActivity;
        private string travelCreditLearnAboutText = string.Empty;
        private string travelCertificateHeaderForTermsConditions;
        private List<MOBTypeOption> travelCertificateTermsConditions;
        private string electronicTravelCertificateHeader = string.Empty;
        private string futureFlightCreditHeader = string.Empty;
        private MOBFutureFlightCreditDetails futureFlightCreditDetails;

        public List<MOBTravelCertificateOrFlightCredit> TravelCertificateOrFlightCreditActivity
        {
            get
            {
                return this.travelCertificateOrFlightCreditActivity;
            }
            set
            {
                this.travelCertificateOrFlightCreditActivity = value;
            }
        }

        public string TravelCertificateHeaderForTermsConditions
        {
            get
            {
                return this.travelCertificateHeaderForTermsConditions;
            }
            set
            {
                this.travelCertificateHeaderForTermsConditions = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBTypeOption> TravelCertificateTermsConditions
        {
            get
            {
                return this.travelCertificateTermsConditions;
            }
            set
            {
                this.travelCertificateTermsConditions = value;
            }
        }

        public string TravelCreditAvailableText
        {
            get
            {
                return this.travelCreditAvailableText;
            }
            set
            {
                this.travelCreditAvailableText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TravelCreditPageTitle
        {
            get
            {
                return this.travelCreditPageTitle;
            }
            set
            {
                this.travelCreditPageTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TravelCreditHeaderTitle
        {
            get
            {
                return this.travelCreditHeaderTitle;
            }
            set
            {
                this.travelCreditHeaderTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TravelCreditHeaderBody
        {
            get
            {
                return this.travelCreditHeaderBody;
            }
            set
            {
                this.travelCreditHeaderBody = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TravelCreditLearnAboutText
        {
            get
            {
                return this.travelCreditLearnAboutText;
            }
            set
            {
                this.travelCreditLearnAboutText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public MOBFutureFlightCreditDetails FutureFlightCreditDetails
        {
            get
            {
                return this.futureFlightCreditDetails;
            }
            set
            {
                this.futureFlightCreditDetails = value;
            }
        }
        public string ElectronicTravelCertificateHeader
        {
            get
            {
                return this.electronicTravelCertificateHeader;
            }
            set
            {
                this.electronicTravelCertificateHeader = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FutureFlightCreditHeader
        {
            get
            {
                return this.futureFlightCreditHeader;
            }
            set
            {
                this.futureFlightCreditHeader = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

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

        [Serializable()]
        public class MOBETCDetail
        {
            private string key = string.Empty;
            private List<string> value;

            public string Key
            {
                get
                {
                    return this.key;
                }
                set
                {
                    this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }

            public List<string> Value
            {
                get
                {
                    return this.value;
                }
                set
                {
                    this.value = value;
                }
            }
        }

        [Serializable()]
        public class MOBFutureFlightCreditDetails
        {
            private string pnr = string.Empty;
            private string lastName = string.Empty;
            private string text = string.Empty;
            private string btnText = string.Empty;
            private string multiFFCText = string.Empty;
            private string multiFFCBtnText = string.Empty;

            private List<MOBCancelledFFCPNRDetails> cancelledFFCPNRList;

            public List<MOBCancelledFFCPNRDetails> CancelledFFCPNRList
            {
                get { return cancelledFFCPNRList; }
                set { cancelledFFCPNRList = value; }
            }


            public string PNR
            {
                get
                {
                    return this.pnr;
                }
                set
                {
                    this.pnr = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }
            public string LastName
            {
                get
                {
                    return this.lastName;
                }
                set
                {
                    this.lastName = value;
                }
            }
            public string MultiFFCText
            {
                get
                {
                    return this.multiFFCText;
                }
                set
                {
                    this.multiFFCText = value;
                }
            }
            public string MultiFFCBtnText
            {
                get
                {
                    return this.multiFFCBtnText;
                }
                set
                {
                    this.multiFFCBtnText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }

            public string Text
            {
                get
                {
                    return this.text;
                }
                set
                {
                    this.text = value;
                }
            }
            public string BtnText
            {
                get
                {
                    return this.btnText;
                }
                set
                {
                    this.btnText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
                }
            }
        }
    }
}
