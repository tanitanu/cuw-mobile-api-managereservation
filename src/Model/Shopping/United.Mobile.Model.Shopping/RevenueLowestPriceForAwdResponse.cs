using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public  class RevenueLowestPriceForAwardSearchResponse : MOBResponse
    {
        private MOBSHOPShopRequest revenueLowestPriceForAwardSearchRequest;
        private List<MOBFSRAlertMessage> fsrRevenueLowestPriceAlertMessages;
        private string sessionId = string.Empty;
        private string cartId = string.Empty;
        private bool noFlightsWithStops;

        public MOBSHOPShopRequest RevenueLowestPriceForAwardSearchRequest { get; set; }
      
        public List<MOBFSRAlertMessage> FSRRevenueLowestPriceAlertMessages { get; set; }

        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CartId
        {
            get { return cartId; }
            set { cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public bool NoFlightsWithStops { get; set; }

        public RevenueLowestPriceForAwardSearchResponse()
        {
            FSRRevenueLowestPriceAlertMessages = new List<MOBFSRAlertMessage>();
        }

    }
}
