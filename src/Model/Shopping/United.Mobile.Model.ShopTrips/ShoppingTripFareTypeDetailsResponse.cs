using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ShoppingTripFareTypeDetailsResponse : MOBResponse
    {
        public List<MOBSHOPShoppingProduct> Columns { get; set; }
        public ShoppingTripFareTypeDetailsResponse()
        {
            Columns = new List<MOBSHOPShoppingProduct>();
        }
    }
}
