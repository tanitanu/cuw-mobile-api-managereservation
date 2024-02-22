using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BagTrackingRequest : MOBRequest
    {
        private string bagTagNum = string.Empty;
        private int pastDays = 0;
        private string pnr = string.Empty;
        private string lastName = string.Empty;
        private string firstName = string.Empty;
        private string mpNumber = string.Empty;
        private string version = string.Empty;
        private string kioskStation = string.Empty;

        public BagTrackingRequest()
            : base()
        {
        }

        public string KioskStation
        {
            get
            {
                return this.kioskStation;
            }
            set
            {
                this.kioskStation = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string BagTagNum
        {
            get
            {
                return this.bagTagNum;
            }
            set
            {
                this.bagTagNum = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int PastDays
        {
            get
            {
                return this.pastDays;
            }
            set
            {
                this.pastDays = value;
            }
        }

        public string PNR
        {
            get
            {
                return this.pnr;
            }
            set
            {
                this.pnr= string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }
            set
            {
                this.version = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PaxLastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PaxFirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MPNumber
        {
            get
            {
                return this.mpNumber;
            }
            set
            {
                this.mpNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
