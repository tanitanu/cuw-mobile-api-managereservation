using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.FareLock
{
    [Serializable()]
    public class MOBSHOPRegisterFareLockResponse : MOBResponse
    {
        private MOBSHOPRegisterFareLockRequest request;
        private MOBSHOPAvailability availability;
        private List<string> disclaimer;

        public MOBSHOPRegisterFareLockRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        public MOBSHOPAvailability Availability
        {
            get
            {
                return this.availability;
            }
            set
            {
                this.availability = value;
            }
        }

        public List<string> Disclaimer
        {
            get
            {
                return this.disclaimer;
            }
            set
            {
                this.disclaimer = value;
            }
        }
    }
}
