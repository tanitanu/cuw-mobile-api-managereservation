using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Service.Presentation.CommonModel;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPPNRs
    {
        private string mileagePlusNumber = string.Empty;
        private List<PNR> current;
        private List<PNR> past;
        private List<PNR> cancelled;
        private List<PNR> inactive;
        private bool gotException4GetPNRSbyMPCSLcallDoNotDropExistingPnrsInWallet = false;

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

        public List<PNR> Current
        {
            get
            {
                return this.current;

            }
            set
            {
                this.current = value;
            }
        }

        public List<PNR> Past
        {
            get
            {
                return this.past;
            }
            set
            {
                this.past = value;
            }
        }

        public List<PNR> Cancelled
        {
            get
            {
                return this.cancelled;
            }
            set
            {
                this.cancelled = value;
            }
        }

        public List<PNR> Inactive
        {
            get
            {
                return this.inactive;
            }
            set
            {
                this.inactive = value;
            }
        }
        public bool GotException4GetPNRSbyMPCSLcallDoNotDropExistingPnrsInWallet
        {
            get { return gotException4GetPNRSbyMPCSLcallDoNotDropExistingPnrsInWallet; }
            set { gotException4GetPNRSbyMPCSLcallDoNotDropExistingPnrsInWallet = value; }
        }
    }
}
