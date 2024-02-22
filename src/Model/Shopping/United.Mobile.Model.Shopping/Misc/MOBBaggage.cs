using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class Baggage
    {
        private string bagTerminal = string.Empty;
        private string bagClaimUnit = string.Empty;
        private bool hasBagLocation;

        public string BagTerminal
        {
            get { return this.bagTerminal; }
            set { this.bagTerminal = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string BagClaimUnit
        {
            get { return this.bagClaimUnit; }
            set { this.bagClaimUnit = value; }
        }

        public bool HasBagLocation
        {
            get { return this.hasBagLocation; }
            set { this.hasBagLocation = value; }
        }
    }
}
