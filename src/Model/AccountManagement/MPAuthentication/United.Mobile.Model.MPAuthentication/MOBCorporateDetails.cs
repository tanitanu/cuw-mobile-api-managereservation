using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBCorporateDetails
    {

        private string discountCode;
        public string DiscountCode
        {
            get { return discountCode; }
            set { discountCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        private string leisureDiscountCode;//For Corporate Leisure Break from business.

        public string LeisureDiscountCode
        {
            get { return leisureDiscountCode; }
            set { leisureDiscountCode = value; }
        }

        private string corporateCompanyName;
        public string CorporateCompanyName
        {
            get { return corporateCompanyName; }
            set { corporateCompanyName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string corporateTravelProvider;
        public string CorporateTravelProvider
        {
            get { return corporateTravelProvider; }
            set { corporateTravelProvider = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareGroupId;

        public string FareGroupId
        {
            get { return fareGroupId; }
            set { fareGroupId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string corporateBookingType;

        public string CorporateBookingType
        {
            get { return corporateBookingType; }
            set { corporateBookingType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private int noOfTravelers;

        public int NoOfTravelers
        {
            get { return noOfTravelers; }
            set { noOfTravelers = value; }
        }

    }
}
