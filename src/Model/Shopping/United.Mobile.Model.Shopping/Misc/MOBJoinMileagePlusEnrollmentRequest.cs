using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class JoinMileagePlusEnrollmentRequest : MOBRequest
    {
        #region Private Properties
        private string sessionId = string.Empty;
        private string email = string.Empty;
        private bool consentToReceiveMarketingEmails = false;
        private string sharesPosition = string.Empty;
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private string flow = string.Empty;
        private bool isGetPNRByRecordLocatorCall = false;
        private bool isGetPNRByRecordLocator = false;
        private string travelerName = string.Empty;
       


        #endregion

        #region Public Properties
        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string Email { get { return this.email; } set { this.email = value; } }

        public bool ConsentToReceiveMarketingEmails { get { return this.consentToReceiveMarketingEmails; } set { this.consentToReceiveMarketingEmails = value; } }

        public string SharesPosition { get { return this.sharesPosition; } set { this.sharesPosition = value; } }
        public string RecordLocator { get { return this.recordLocator; } set { this.recordLocator = value; } }
        public string LastName { get { return this.lastName; } set { this.lastName = value; } }
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

        public bool IsGetPNRByRecordLocatorCall { get { return this.isGetPNRByRecordLocatorCall; } set { this.isGetPNRByRecordLocatorCall = value; } }

        public bool IsGetPNRByRecordLocator { get { return this.isGetPNRByRecordLocator; } set { this.isGetPNRByRecordLocator = value; } }

        public string TravelerName
        {
            get
            {
                return this.travelerName;
            }
            set
            {
                this.travelerName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }     

        #endregion
    }
}
