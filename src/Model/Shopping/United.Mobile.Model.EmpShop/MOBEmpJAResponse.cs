using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.MPSignIn;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpJAResponse : MOBResponse
    {

        private bool allowImpersonation;
        private MOBEmpJA empJA;
        private MOBEmpJARequest empJARequest;
        private string impersonateType;
        private MOBEmpJA loggedInJA;
        private MOBEmpPassRiderExtended loggedInPassRider;
        private MOBEmployeeProfileExtended empProfileExtended;

        public bool AllowImpersonation
        {
            get
            {
                return allowImpersonation;
            }
            set
            {
                allowImpersonation = value;
            }
        }


        public MOBEmpJA EmpJA
        {
            get
            {
                return empJA;
            }
            set
            {
                empJA = value;
            }

        }
        public MOBEmpJARequest EmpJARequest
        {
            get
            {
                return empJARequest;
            }
            set
            {
                empJARequest = value;
            }
        }
        public string ImpersonateType
        {
            get
            {
                return impersonateType;
            }
            set
            {
                impersonateType = value;
            }
        }
        public MOBEmpJA LoggedInJA
        {
            get
            {
                return loggedInJA;
            }
            set
            {
                loggedInJA = value;
            }
        }
        public MOBEmpPassRiderExtended LoggedInPassRider
        {
            get
            {
                return loggedInPassRider;
            }
            set
            {
                loggedInPassRider = value;
            }
        }
        public MOBEmployeeProfileExtended EmpProfileExtended
        {
            get
            {
                return empProfileExtended;
            }
            set
            {
                empProfileExtended = value;
            }
        }
    }
}
