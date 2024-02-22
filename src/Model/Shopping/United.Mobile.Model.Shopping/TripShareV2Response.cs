using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class TripShareV2Response:MOBResponse
    {
        public MOBSHOPShopRequest ShopRequest { get; set; }
       
    }
}
