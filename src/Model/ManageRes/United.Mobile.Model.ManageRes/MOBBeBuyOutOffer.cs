using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.ManageRes
{
    [Serializable()]
    public class MOBBeBuyOutOffer
    {
        private string title;
        private string header;
        private List<MOBSHOPOption> elfShopOptions;
        private decimal price;
        private string currencyCode;
        private string text1;
        private string text2;
        private string button;
        private string changeFeesText;


        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

        public List<MOBSHOPOption> ELFShopOptions
        {
            get
            {
                return this.elfShopOptions;
            }
            set
            {
                this.elfShopOptions = value;
            }
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
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

        public string Button
        {
            get { return button; }
            set { button = value; }
        }

        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }

        public string ChangeFeesText
        {
            get { return changeFeesText; }
            set { changeFeesText = value; }
        }
    }
}
