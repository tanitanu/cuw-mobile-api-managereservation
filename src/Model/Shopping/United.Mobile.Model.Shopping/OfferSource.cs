using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class OfferSource
    {
        private string offerHeaderDescription = string.Empty;
        private ClubDayPassOffer clubDayPassOffer;
        private ProductOffer productOffer;

        public string OfferHeaderDescription
        {
            get
            {
                return this.offerHeaderDescription;
            }
            set
            {
                this.offerHeaderDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public ClubDayPassOffer ClubDayPassOffer { get; set; } 

        public ProductOffer ProductOffer { get; set; } 
    }
}
