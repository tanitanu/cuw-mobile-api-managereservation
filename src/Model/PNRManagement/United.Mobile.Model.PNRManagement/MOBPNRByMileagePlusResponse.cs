using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable()]
    public class MOBPNRByMileagePlusResponse : MOBResponse
    {
        private string mileagePlusNumber = string.Empty;
        private MOBReservationType reservationType;
        private MOBMPPNRs pnrs;

        public MOBPNRByMileagePlusResponse()
            : base()
        {
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

        public MOBReservationType ReservationType
        {
            get
            {
                return this.reservationType;
            }
            set
            {
                this.reservationType = value;
            }
        }

        public MOBMPPNRs PNRs
        {
            get
            {
                return this.pnrs;
            }
            set
            {
                this.pnrs = value;
            }
        }
    }
}
