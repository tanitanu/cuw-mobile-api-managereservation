using System;
using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class MOBBundleTile
    {
        private string offerTitle;
        private List<string> offerDescription;
        private string priceText;
        private bool isSelected;
        private int tileIndex;
        private string offerPrice;

        public string OfferTitle
        {
            get { return offerTitle; }
            set { offerTitle = value; }
        }

        public List<string> OfferDescription
        {
            get { return offerDescription; }
            set { offerDescription = value; }
        }

        public string PriceText
        {
            get { return priceText; }
            set { priceText = value; }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        public int TileIndex
        {
            get { return tileIndex; }
            set { tileIndex = value; }
        }
        public string OfferPrice
        {
            get { return offerPrice; }
            set { offerPrice = value; }
        }

    }

}
