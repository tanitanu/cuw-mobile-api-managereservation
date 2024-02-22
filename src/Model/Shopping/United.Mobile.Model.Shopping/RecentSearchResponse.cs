using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class RecentSearchResponse : MOBResponse
    {
        public List<MOBSHOPShopRequest> ShopRequests { get; set; }

        public RecentSearchResponse()
        {
            ShopRequests = new List<MOBSHOPShopRequest>();
        }
    }
}
