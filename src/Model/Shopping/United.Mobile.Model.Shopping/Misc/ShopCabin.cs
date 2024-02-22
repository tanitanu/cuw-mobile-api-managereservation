using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopCabin
    {
        public ShopBoardingTotal BoardingTotals { get; set; } 
        public List<ShopMeal> Meals { get; set; } 
        public string Type { get; set; } = string.Empty;
        public ShopCabin()
        {
            Meals = new List<ShopMeal>();
        }
    }
}
