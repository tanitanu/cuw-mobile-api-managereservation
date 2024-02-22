using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBPlusPoints
    {
        private string plusPointsAvailableText;
        private string plusPointsAvailableValue;
        private string plusPointsDeductedText;
        private string plusPointsDeductedValue;
        private string plusPointsExpirationText;
        private string plusPointsExpirationValue;
        private string plusPointsUpgradesText;
        private string plusPointsUpgradesLink;
        private bool isHidePlusPointsExpiration;
        private string plusPointsExpirationInfo;
        private string plusPointsExpirationInfoHeader;
        private string plusPointsExpirationPointsInfoSubHeader;
        private string plusPointsExpirationInfoDateSubHeader;
        private List<MOBKVP> expirationPointsAndDatesKVP;
        private string webShareToken = string.Empty;
        private string webSessionShareUrl = string.Empty;
        private Boolean redirectToDotComMyTripsWithSSOCheck;
        public string PlusPointsAvailableText
        {
            get { return this.plusPointsAvailableText; }
            set { this.plusPointsAvailableText = value; }
        }
        public string PlusPointsAvailableValue
        {
            get { return this.plusPointsAvailableValue; }
            set { this.plusPointsAvailableValue = value; }
        }
        public string PlusPointsDeductedText
        {
            get { return this.plusPointsDeductedText; }
            set { this.plusPointsDeductedText = value; }
        }
        public string PlusPointsDeductedValue
        {
            get { return this.plusPointsDeductedValue; }
            set { this.plusPointsDeductedValue = value; }
        }
        public string PlusPointsExpirationText
        {
            get { return this.plusPointsExpirationText; }
            set { this.plusPointsExpirationText = value; }
        }
        public string PlusPointsExpirationValue
        {
            get { return this.plusPointsExpirationValue; }
            set { this.plusPointsExpirationValue = value; }
        }
        public string PlusPointsUpgradesText
        {
            get { return this.plusPointsUpgradesText; }
            set { this.plusPointsUpgradesText = value; }
        }
        public string PlusPointsUpgradesLink
        {
            get { return this.plusPointsUpgradesLink; }
            set { this.plusPointsUpgradesLink = value; }
        }
        public bool IsHidePlusPointsExpiration
        {
            get { return this.isHidePlusPointsExpiration; }
            set { this.isHidePlusPointsExpiration = value; }
        }
        public string PlusPointsExpirationInfo
        {
            get { return this.plusPointsExpirationInfo; }
            set { this.plusPointsExpirationInfo = value; }
        }
        public string PlusPointsExpirationInfoHeader
        {
            get { return this.plusPointsExpirationInfoHeader; }
            set { this.plusPointsExpirationInfoHeader = value; }
        }
        public string PlusPointsExpirationInfoPointsSubHeader
        {
            get { return this.plusPointsExpirationPointsInfoSubHeader; }
            set { this.plusPointsExpirationPointsInfoSubHeader = value; }
        }
        public string PlusPointsExpirationInfoDateSubHeader
        {
            get { return this.plusPointsExpirationInfoDateSubHeader; }
            set { this.plusPointsExpirationInfoDateSubHeader = value; }
        }
        public List<MOBKVP> ExpirationPointsAndDatesKVP
        {
            get { return this.expirationPointsAndDatesKVP; }
            set { this.expirationPointsAndDatesKVP = value; }
        }
        public string WebShareToken
        {
            get { return this.webShareToken; }
            set { this.webShareToken = value; }
        }
        public string WebSessionShareUrl
        {
            get { return this.webSessionShareUrl; }
            set { this.webSessionShareUrl = value; }
        }
        public Boolean RedirectToDotComMyTripsWithSSOCheck
        {
            get { return this.redirectToDotComMyTripsWithSSOCheck; }
            set { this.redirectToDotComMyTripsWithSSOCheck = value; }
        }
    }
}
