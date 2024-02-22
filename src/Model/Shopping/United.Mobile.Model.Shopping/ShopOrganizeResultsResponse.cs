using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ShopOrganizeResultsResponse :MOBResponse
    {
        private ShopOrganizeResultsReqeust organizeResultsRequest;
        
        public ShopOrganizeResultsReqeust OrganizeResultsRequest
        {
            get
            {
                return this.organizeResultsRequest;
            }
            set
            {
                this.organizeResultsRequest = value;
            }
        }


        public MOBSHOPAvailability Availability { get; set; } 

        public List<string> Disclaimer { get; set; } 

        public string CartId { get; set; } = string.Empty;

        public MOBSHOPShopRequest ShopRequest { get; set; }
        public ShopOrganizeResultsResponse()
        {
            Disclaimer = new List<string>();
        }

    }
}
