using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBMPPINPWDSecurityUpdateDetails
    {
        public MOBMPPINPWDSecurityUpdateDetails()
            : base()
        {
        }
        private MOBMPSecurityUpdatePath mpSecurityPath;
        private int daysToCompleteSecurityUpdate;
        private bool passwordOnlyAllowed; // passwordOnlyAllowed says 
        private bool updateLaterAllowed;
        private bool forceSignOut; // This is to force sign out when update later is disabled as its time to update the Security Data and will be forced to update data to move forward (As of now here too other than Revenue Booking)
        public bool ForceSignOut
        {
            get { return this.forceSignOut; }
            set { this.forceSignOut = value; }
        }
        private List<MOBMPSecurityUpdatePath> mpSecurityPathList;
        private MOBMPPINPWDSecurityItems securityItems;
        private bool isPinPwdAutoSignIn = false;

        //The "daysToCompleteSecurityUpdate" property communicates how many days the user has to complete the security updates
        //The "passwordOnlyAllowed" property communicates that only their password can be used to sign-in.  If this property is true then check the value entered by the user in the PIN/Password input field.  If the user entered a 4 digit value then log them out and display an error message.
        //The "updateLaterAllowed" property communicates whether the user can perform the security updates later or not.


        public MOBMPPINPWDSecurityItems SecurityItems
        {
            get { return securityItems; }
            set { securityItems = value; }
        }

        public MOBMPSecurityUpdatePath MPSecurityPath
        {
            get
            {
                return this.mpSecurityPath;
            }
            set
            {
                this.mpSecurityPath = value;
            }
        }

        public int DaysToCompleteSecurityUpdate
        {
            get
            {
                return this.daysToCompleteSecurityUpdate;
            }
            set
            {
                this.daysToCompleteSecurityUpdate = value;
            }
        }

        public bool PasswordOnlyAllowed
        {
            get { return this.passwordOnlyAllowed; }
            set { this.passwordOnlyAllowed = value; }
        }

        public bool UpdateLaterAllowed
        {
            get { return this.updateLaterAllowed; }
            set { this.updateLaterAllowed = value; }
        }

        public List<MOBMPSecurityUpdatePath> MPSecurityPathList
        {
            get
            {
                return this.mpSecurityPathList;
            }
            set
            {
                this.mpSecurityPathList = value;
            }
        }

        private List<MOBItem> securityUpdateMessages;
        public List<MOBItem> SecurityUpdateMessages
        {
            get
            {
                return this.securityUpdateMessages;
            }
            set
            {
                this.securityUpdateMessages = value;
            }
        }

        //TFS Backlog Defect #27502 - PINPWD AutoSignIn
        public bool IsPinPwdAutoSignIn
        {
            get { return this.isPinPwdAutoSignIn; }
            set { this.isPinPwdAutoSignIn = value; }
        }


    }
}
