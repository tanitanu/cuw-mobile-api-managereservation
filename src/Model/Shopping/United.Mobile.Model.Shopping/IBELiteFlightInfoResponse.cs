using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class GetProductInfoForFSRDResponse :MOBResponse
    {
        public List<MOBItem> IBELiteShopMessages { get; set; } 

        public List<Option> IBELiteShopOptions { get; set; } 

        public List<MOBSHOPShoppingProduct> ShoppingProducts { get; set; }
        public GetProductInfoForFSRDResponse()
        {
            IBELiteShopMessages = new List<MOBItem>();
            IBELiteShopOptions = new List<Option>();
            ShoppingProducts = new List<MOBSHOPShoppingProduct>();
        }
    }
}
