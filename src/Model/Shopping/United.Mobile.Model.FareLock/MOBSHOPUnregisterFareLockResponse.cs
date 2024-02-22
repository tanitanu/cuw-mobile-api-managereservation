using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Model.FareLock
{
    [Serializable()]
    public class MOBSHOPUnregisterFareLockResponse : MOBResponse
    {
        private MOBSHOPUnregisterFareLockRequest request;
        private MOBSHOPAvailability availability;

        public MOBSHOPUnregisterFareLockRequest Request
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
    }
}

