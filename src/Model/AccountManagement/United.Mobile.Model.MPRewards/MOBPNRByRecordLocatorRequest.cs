using United.Mobile.Model.Common;
using System.Collections.Generic;

namespace United.Mobile.Model.MPRewards
{
    public class MOBPNRByRecordLocatorRequest : MOBRequest
    {
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private string sessionId = string.Empty;
        private string hashKey = string.Empty;
        private string flow = string.Empty;
        private bool isOTFConversion;
        private bool isRefreshedUserData;
        private string encryptedRequest;
        private string requestor;
        private List<MOBItem> catalogValues;
        private bool isAddTriptoMyAccount;

        public string EncryptedRequest
        {
            get { return this.encryptedRequest; }
            set { this.encryptedRequest = value; }
        }

        public string Requestor
        {
            get { return this.requestor; }
            set { this.requestor = value; }
        }

        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public bool IsOTFConversion
        {
            get { return this.isOTFConversion; }
            set { this.isOTFConversion = value; }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }



        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string HashKey
        {
            get
            {
                return this.hashKey;
            }
            set
            {
                this.hashKey = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string Flow
        {
            get
            {
                return this.flow;
            }
            set
            {
                this.flow = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public bool IsRefreshedUserData
        {
            get { return this.isRefreshedUserData; }
            set { this.isRefreshedUserData = value; }
        }

        public List<MOBItem> CatalogValues
        {
            get
            {
                return this.catalogValues;
            }
            set
            {
                this.catalogValues = value;
            }
        }

        public bool IsAddTriptoMyAccount
        {
            get { return this.isAddTriptoMyAccount; }
            set { this.isAddTriptoMyAccount = value; }
        }
    }
}
