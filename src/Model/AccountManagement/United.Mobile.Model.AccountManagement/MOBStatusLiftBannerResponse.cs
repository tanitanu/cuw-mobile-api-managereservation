using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBStatusLiftBannerResponse : MOBResponse
    {
        private string currentLevel;
        private string currentLevelName;
        private string eligibleLevel;
        private string eligibleLevelName;
        private string eligibleLevelCode;
        private string description;
        private string statusLiftURL;
        private string mileagePlusNumber;
        private string custID;
        private string promoCode;
        private DateTime expirationDate;

        public string CurrentLevel
        {
            get { return currentLevel; }
            set { currentLevel = value; }
        }
        public string CurrentLevelName
        {
            get { return currentLevelName; }
            set { currentLevelName = value; }
        }
        public string EligibleLevel
        {
            get { return eligibleLevel; }
            set { eligibleLevel = value; }
        }
        public string EligibleLevelName
        {
            get { return eligibleLevelName; }
            set { eligibleLevelName = value; }
        }
        public string EligibleLevelCode
        {
            get { return eligibleLevelCode; }
            set { eligibleLevelCode = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string StatusLiftURL
        {
            get { return statusLiftURL; }
            set { statusLiftURL = value; }
        }
        public string MileagePlusNumber
        {
            get { return mileagePlusNumber; }
            set { mileagePlusNumber = value; }
        }
        public string CustID
        {
            get { return custID; }
            set { custID = value; }
        }
        public string PromoCode
        {
            get { return promoCode; }
            set { promoCode = value; }
        }
        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }
    }
}
