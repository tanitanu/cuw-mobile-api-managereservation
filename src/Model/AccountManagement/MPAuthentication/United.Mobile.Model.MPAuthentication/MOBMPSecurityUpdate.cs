using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBMPSecurityUpdate
    {
        public MOBMPSecurityUpdate()
            : base()
        {
        }
        private List<MOBMPSecurityUpdatePath> mpSecurityPathList;
        private int daysToCompleteSecurityUpdate;
        private bool passwordOnlyAllowed;
        private bool updateLaterAllowed;

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

    }
}
