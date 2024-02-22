using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable()]
    public class MOBPNRByMileagePlusRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;

        public MOBPNRByMileagePlusRequest()
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

        public  MOBReservationType ReservationType { get; set; }
    }
}
