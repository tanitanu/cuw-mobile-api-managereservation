using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable()]
    public class MOBPNRByRecordLocatorRequest : MOBRequest
    {
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private string sessionId = string.Empty;
        private string hashKey = string.Empty;
        private string flow = string.Empty;//
        private bool isOTFConversion;
        private bool isRefreshedUserData;
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
    }
}
