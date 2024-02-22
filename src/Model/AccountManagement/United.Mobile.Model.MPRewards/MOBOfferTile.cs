using System;

namespace United.Mobile.Model.MPRewards
{
    [Serializable()]
    public class MOBOfferTile
    {
        private string title;
        private string text1;
        private string text2;
        private string text3;
        private decimal price;
        private string currencyCode;
        private Int32 miles;
        private string displayMiles;
        private bool showUpliftPerMonthPrice;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Text1
        {
            get { return text1; }
            set { text1 = value; }
        }

        public string Text2
        {
            get { return text2; }
            set { text2 = value; }
        }

        public string Text3
        {
            get { return text3; }
            set { text3 = value; }
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }
        public Int32 Miles
        {
            get { return miles; }
            set { miles = value; }
        }
        public string DisplayMiles
        {
            get { return displayMiles; }
            set { displayMiles = value; }

        }

        public bool ShowUpliftPerMonthPrice
        {
            get { return showUpliftPerMonthPrice; }
            set { showUpliftPerMonthPrice = value; }
        }
    }
}
