using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class PriceSummary
    {
        private string cheapestAirfare = string.Empty;
        private string cheapestAirfareNoConnections = string.Empty;
        private string cheapestAirfareWithConnections = string.Empty;
        private string cheapestRefundable = string.Empty;
        private string mostExpensiveAirfare = string.Empty;
        private string cheapestPartnerAirfareWithConnections = string.Empty;

        public string CheapestAirfare
        {
            get
            {
                return this.cheapestAirfare;
            }
            set
            {
                this.cheapestAirfare = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CheapestAirfareNoConnections
        {
            get
            {
                return this.cheapestAirfareNoConnections;
            }
            set
            {
                this.cheapestAirfareNoConnections = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CheapestAirfareWithConnections
        {
            get
            {
                return this.cheapestAirfareWithConnections;
            }
            set
            {
                this.cheapestAirfareWithConnections = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CheapestRefundable
        {
            get
            {
                return this.cheapestRefundable;
            }
            set
            {
                this.cheapestRefundable = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MostExpensiveAirfare
        {
            get
            {
                return this.mostExpensiveAirfare;
            }
            set
            {
                this.mostExpensiveAirfare = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CheapestPartnerAirfareWithConnections
        {
            get
            {
                return this.cheapestPartnerAirfareWithConnections;
            }
            set
            {
                this.cheapestPartnerAirfareWithConnections = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
