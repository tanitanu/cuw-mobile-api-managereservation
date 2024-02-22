using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    [XmlRoot("MOBTPIInfoInBookingPath")]
    public class TPIInfoInBookingPath
    {
        private List<MOBItem> TPIAIGReturnedMessageContentList; //Covi5d-19 Emergency WHO TPI content
        [XmlArray("TPIAIGReturnedMessageContentList")]
        public List<MOBItem> tpiAIGReturnedMessageContentList
        {
            get
            {
                return this.TPIAIGReturnedMessageContentList;
            }
            set
            {
                this.TPIAIGReturnedMessageContentList = value;
            }
        }

        public string QuoteId { get; set; }

        public double Amount { get; set; }

        public string Title { get; set; } 

        public string Header { get; set; } 

        public List<string> Content { get; set; }

        public string Tnc { get; set; } 

        public string CoverCostText { get; set; } = string.Empty;

        public string CoverCost { get; set; } = string.Empty;

        public string CoverCostStatus { get; set; } = string.Empty;

        public string Img { get; set; } 

        public string ButtonTextInProdPage { get; set; } 

        public string ButtonTextInRTIPage { get; set; }

        public bool IsRegistered { get; set; }

        public string LegalInformation { get; set; } 

        public string LegalInformationText { get; set; } 

        public string PopUpMessage { get; set; } = string.Empty;

        public double OldAmount { get; set; }

        public string OldQuoteId { get; set; } = string.Empty;

        public string TncSecondaryFOPPage { get; set; } 

        public string PaymentContent { get; set; }

        public string PaymentContentHeader { get; set; }

        public string PaymentContentBody { get; set; }

        public string DisplayAmount { get; set; } 

        public string ConfirmationMsg { get; set; } 

        public string ConfirmationEmailForTPIPurcahse { get; set; }

        public bool IsTPIIncludedInCart { get; set; }

        public TPIInfoInBookingPath()
        {
            tpiAIGReturnedMessageContentList = new List<MOBItem>();
            Content = new List<string>();
        }

        private string tileTitle1;
        private string tileTitle2;
        private string tileImage;
        private string tileLinkText;

        public string TileTitle1
        {
            get
            {
                return this.tileTitle1;
            }
            set
            {
                this.tileTitle1 = string.IsNullOrEmpty(value) ? string.Empty : value;
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
                this.tileTitle2 = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string TileImage
        {
            get
            {
                return this.tileImage;
            }
            set
            {
                this.tileImage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string TileLinkText
        {
            get
            {
                return this.tileLinkText;
            }
            set
            {
                this.tileLinkText = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

    }
}
