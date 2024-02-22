using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBSHOPDataCarrier
    {
        public string SearchType { get; set; } = string.Empty;
       
        public string PriceFormText { get; set; } = string.Empty;
       
        public decimal FsrMinPrice { get; set; } 

    }
}
