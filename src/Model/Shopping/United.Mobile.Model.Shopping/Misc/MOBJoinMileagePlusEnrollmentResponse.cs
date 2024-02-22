using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class JoinMileagePlusEnrollmentResponse : MOBResponse
    {
        #region Private Properties
        private string email = string.Empty;
        private List<MOBKVP> enrolledUserInfo = null;        
        private string enrollAnotherTravelerButtonText = string.Empty;        
        private string sessionId= string.Empty;
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private bool isGetPNRByRecordLocatorCall = false;
        private bool isGetPNRByRecordLocator = false;
        private bool isEnrollAnotherTraveler = false;
        private string accountConfirmationTitle = string.Empty;
        private string accountConfirmationHeader = string.Empty;
        private string accountConfirmationBody = string.Empty;
        private string closeButtion = string.Empty;
        private string accountCreatedText = string.Empty;
        private string deviceId = string.Empty;


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

        //"CustomerId":164345762,"LoyaltyId":"ADG61449","AlreadyEnrolled":true,

        public string EnrollAnotherTravelerButtonText { get { return this.enrollAnotherTravelerButtonText; } set { this.enrollAnotherTravelerButtonText = value; } }
        public string AccountConfirmationTitle { get { return this.accountConfirmationTitle; } set { this.accountConfirmationTitle = value; } }
        public string AccountConfirmationHeader { get { return this.accountConfirmationHeader; } set { this.accountConfirmationHeader = value; } }
        public string AccountConfirmationBody { get { return this.accountConfirmationBody; } set { this.accountConfirmationBody = value; } }
        public string RecordLocator { get { return this.recordLocator; } set { this.recordLocator = value; } }
        public string LastName { get { return this.lastName; } set { this.lastName = value; } }
        public bool IsGetPNRByRecordLocatorCall { get { return this.isGetPNRByRecordLocatorCall; } set { this.isGetPNRByRecordLocatorCall = value; } }
        public bool IsGetPNRByRecordLocator { get { return this.isGetPNRByRecordLocator; } set { this.isGetPNRByRecordLocator = value; } }
        public string CloseButtion { get { return this.closeButtion; } set { this.closeButtion = value; } }
        public string AccountCreatedText { get { return this.accountCreatedText; } set { this.accountCreatedText = value; } }
        public string DeviceId { get { return this.deviceId; } set { this.deviceId = value; } }

        public List<MOBKVP>  EnrolledUserInfo
        {
            get
            {
                return this.enrolledUserInfo;
            }
            set
            {
                this.enrolledUserInfo = value;
            }
        }
        public bool IsEnrollAnotherTraveler { get { return this.isEnrollAnotherTraveler; } set { this.isEnrollAnotherTraveler = value; } }       
        #endregion
    }
}
