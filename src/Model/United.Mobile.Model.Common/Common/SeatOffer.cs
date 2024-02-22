using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  United.Mobile.Model.Common
{
    [Serializable]
    public class SeatOffer
    {
        public string OfferTitle { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string CurrencyCode { get; set; } = string.Empty;
        public string OfferText1 { get; set; } = string.Empty;
        public string OfferText2 { get; set; } = string.Empty;
        public string OfferText3 { get; set; } = string.Empty;
        public bool IsAdvanceSeatOffer { get; set; }
        public Int32 Miles { get; set; }
        public string DisplayMiles { get; set; } 

    }
}
