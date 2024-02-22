using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class TPIInfo
    {
        public List<MOBItem> TPIAIGReturnedMessageContentList { get; set; }

        public string ProductCode { get; set; } = string.Empty;

        public string ProductName { get; set; } = string.Empty;

        public string DisplayAmount { get; set; } = string.Empty;

        public string FormattedDisplayAmount { get; set; } = string.Empty;

        public double Amount { get; set; }

        public string CoverCost { get; set; } = string.Empty;

        public string PageTitle { get; set; } = string.Empty;

        public string Title1 { get; set; } = string.Empty;

        public string Title2 { get; set; } = string.Empty;

        public string Title3 { get; set; } = string.Empty;

        public string QuoteTitle { get; set; } = string.Empty;

        public string Headline1 { get; set; } = string.Empty;

        public string Headline2 { get; set; } = string.Empty;

        public string Body1 { get; set; } = string.Empty;

        public string Body2 { get; set; } = string.Empty;

        public string Body3 { get; set; } = string.Empty;

        public string LineItemText { get; set; } = string.Empty;

        public string TNC { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;


        public string PaymentContent { get; set; } = string.Empty;

        public string PkDispenserPublicKey { get; set; } = string.Empty;

        public string Confirmation1 { get; set; } = string.Empty;

        public string Confirmation2 { get; set; } = string.Empty;

        private string tileImage = string.Empty;
        private string tileTitle1 = string.Empty;
        private string tileTitle2 = string.Empty;
        private string tileQuoteTitle = string.Empty;
        public string TileImage
        {
            get
            {
                return this.tileImage;
            }
            set
            {
                this.tileImage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        private string tileLinkText;
        public string TileLinkText
        {
            get
            {
                return this.tileLinkText;
            }
            set
            {
                this.tileLinkText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TileTitle1
        {
            get
            {
                return this.tileTitle1;
            }
            set
            {
                this.tileTitle1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TileTitle2
        {
            get
            {
                return this.tileTitle2;
            }
            set
            {
                this.tileTitle2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string TileQuoteTitle
        {
            get
            {
                return this.tileQuoteTitle;
            }
            set
            {
                this.tileQuoteTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


    }
}
