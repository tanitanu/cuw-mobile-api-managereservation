using System;
using System.Collections.Generic;


namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class UpsellProduct
    {
        #region Properties
        public string BookingCode { get; set; } = string.Empty;
       
        public decimal TotalPrice { get; set; }
       
        public string CabinType { get; set; } = string.Empty;
      
        public string SolutionId { get; set; } = string.Empty;
        
        public string ProductSubtype { get; set; } = string.Empty;
        
        public string ProductType { get; set; } = string.Empty;

        public string LastSolutionId { get; set; } = string.Empty;    

        public List<string> Prices { get; set; }
        
        public int NumberOfPassengers { get; set; }

        public string LongCabin { get; set; } = string.Empty;

        #endregion
        public UpsellProduct()
        {
            Prices = new List<string>();
        }
    }

}
