using System;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBAcceleratorOffer
    {
        private string id;
        private double price;
        private string formattedPrice;
        private string productCode;
        private string productId;
        private string awardMilesOffer;
        private string selectedAwardMilesText;
        private string subProductCode;
        private MOBPremierAcceleratorOffer premierAcceleratorOffer;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        public string FormattedPrice
        {
            get { return formattedPrice; }
            set { formattedPrice = value; }
        }

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public string ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        public string AwardMilesOffer
        {
            get { return awardMilesOffer; }
            set { awardMilesOffer = value; }
        }

        public string SelectedAwardMilesText
        {
            get { return selectedAwardMilesText; }
            set { selectedAwardMilesText = value; }
        }

        public string SubProductCode
        {
            get { return subProductCode; }
            set { subProductCode = value; }
        }

        public MOBPremierAcceleratorOffer PremierAcceleratorOffer
        {
            get { return premierAcceleratorOffer; }
            set { premierAcceleratorOffer = value; }
        }

    }


}
