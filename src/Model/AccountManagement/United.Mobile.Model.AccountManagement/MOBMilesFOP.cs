using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMilesFOP
    {
        private MOBName name;
        private long customerId ;        
        private string profileOwnerMPAccountNumber = string.Empty;
        private bool hasEnoughMiles;
        private Int32 requiredMiles;
        private Int32 availableMiles;
        private Int32 remainingMiles;
        private string displayRequiredMiles = string.Empty;
        private string displayAvailableMiles = string.Empty;
        private string displayremainingMiles = string.Empty;
        public MOBName Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }
        public long CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }
        public string ProfileOwnerMPAccountNumber
        {
            get
            {
                return this.profileOwnerMPAccountNumber;
            }
            set
            {
                this.profileOwnerMPAccountNumber = value;
            }
        }
        public bool HasEnoughMiles
        {
            get
            {
                return hasEnoughMiles;
            }
            set {
                hasEnoughMiles = value;
            }
           
        }
        public Int32 RequiredMiles
        {
            get
            {
                return this.requiredMiles;
            }
            set
            {
                this.requiredMiles = value;
            }
        }
        public Int32 AvailableMiles
        {
            get
            {
                return this.availableMiles;
            }
            set
            {
                this.availableMiles = value;
            }
        }
        public string DisplayRequiredMiles
        {
            get
            {
                return this.displayRequiredMiles;
            }
            set
            {
                this.displayRequiredMiles = value;
            }
        }
        public string DisplayAvailableMiles
        {
            get
            {
                return this.displayAvailableMiles;
            }
            set
            {
                this.displayAvailableMiles = value;
            }
        }
        public Int32 RemainingMiles
        {
            get
            {
                return this.remainingMiles;
            }
            set
            {
                this.remainingMiles = value;
            }
        }
        public string DisplayRemainingMiles
        {
            get
            {
                return this.displayremainingMiles;
            }
            set
            {
                this.displayremainingMiles = value;
            }
        }
    }       
}
