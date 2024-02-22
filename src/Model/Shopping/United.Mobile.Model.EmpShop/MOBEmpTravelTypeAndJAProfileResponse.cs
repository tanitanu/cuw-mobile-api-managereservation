using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpTravelTypeAndJAProfileResponse : MOBResponse
    {
        private MOBEmpJAResponse mobEmpJAResponse;
        private MOBEmpTravelTypeResponse mobEmpTravelTypeResponse;

        public MOBEmpJAResponse MOBEmpJAResponse
        {
            get
            {
                return this.mobEmpJAResponse;
            }
            set
            {
                mobEmpJAResponse = value;
            }
        }
        public MOBEmpTravelTypeResponse MOBEmpTravelTypeResponse
        {
            get
            {
                return this.mobEmpTravelTypeResponse;
            }
            set
            {
                mobEmpTravelTypeResponse = value;
            }
        }
    }
}
