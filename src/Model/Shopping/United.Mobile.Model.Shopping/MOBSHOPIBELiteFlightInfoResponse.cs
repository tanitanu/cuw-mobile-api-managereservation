using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBSHOPGetProductInfoForFSRDResponse : MOBResponse
    {
        private List<MOBItem> ibeLiteShopMessages;
        private List<Option> ibeLiteShopOptions;
        private List<MOBSHOPShoppingProduct> shoppingProducts;

        public List<MOBItem> IBELiteShopMessages
        {
            get
            {
                return ibeLiteShopMessages;
            }
            set
            {
                ibeLiteShopMessages = value;
            }
        }

        public List<Option> IBELiteShopOptions
        {
            get
            {
                return ibeLiteShopOptions;
            }
            set
            {
                ibeLiteShopOptions = value;
            }
        }

        public List<MOBSHOPShoppingProduct> ShoppingProducts
        {
            get
            {
                return shoppingProducts;
            }
            set
            {
                shoppingProducts = value;
            }
        }
    }
}
