using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class CustomerPreferencesRequest : MOBRequest
    {
        private string sessionId;
        private string customerId;
        private string profileId;
        private string hashValue;
        private string mileagePlusNumber;

        public string HashValue
        {
            get { return this.hashValue; }
            set { this.hashValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MileagePlusNumber
        {
            get { return this.mileagePlusNumber; }
            set { this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SessionId
        {
            get { return this.sessionId; }
            set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string CustomerId
        {
            get { return this.customerId; }
            set { this.customerId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ProfileId
        {
            get { return this.profileId; }
            set { this.profileId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }
}