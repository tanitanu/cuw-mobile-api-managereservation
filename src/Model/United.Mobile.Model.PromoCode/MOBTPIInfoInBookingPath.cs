using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PromoCode
{

    [Serializable]
    public class MOBTPIInfoInBookingPath
    {
        private string quoteId = string.Empty; // TPI price $14.12
        private double amount; // 14.12
        private string title; // Travel Guard® Insurance
        private string header; // Purchase travel insurance to <b>cover the unexpected:</b>
        private List<string> content; // - Trip cancellations, - Missed flight connections, - Lost baggage
        private string tnc;
        private string coverCostText = string.Empty; // Travel insurance coverage is based on total cost of trip
        private string coverCost = string.Empty; // Covers total trip cost of $134.40
        private string coverCostStatus = string.Empty;// (currently added to trip)
        private string img;
        private string buttonTextInProdPage;
        private string buttonTextInRTIPage;
        private bool isRegistered;// current status, show is in your cart or not.
        private string legalInformation;
        private string legalInformationText;
        private string popUpMessage = string.Empty;
        private double oldAmount;
        private string oldQuoteId = string.Empty;
        private string tncSecondaryFOPPage;
        private string paymentContent; // travel insurance 
        private string paymentContentHeader;
        private string paymentContentBody; // DB content 
        private string displayAmount;
        private string confirmationMsg;
        private string confirmationEmailForTPIPurcahse;
        private bool isTPIIncludedInCart = false;
        private List<MOBItem> tpiAIGReturnedMessageContentList;
        private MOBItem policyOfInsuranceTextAndUrl;
        private MOBItem termsAndConditionsTextAndUrl;
        private string tileTitle1;
        private string tileTitle2;
        private string tileImage;
        private string tileLinkText;

        public List<MOBItem> TPIAIGReturnedMessageContentList
        {
            get
            {
                return this.tpiAIGReturnedMessageContentList;
            }
            set
            {
                this.tpiAIGReturnedMessageContentList = value;
            }
        }

        public string QuoteId
        {
            get
            {
                return this.quoteId;
            }
            set
            {
                this.quoteId = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public double Amount
        {
            get
            {
                return this.amount;
            }
            set
            {
                this.amount = value;
            }
        }
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string Header
        {
            get
            {
                return this.header;
            }
            set
            {
                this.header = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public List<string> Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
            }
        }
        public string Tnc
        {
            get
            {
                return this.tnc;
            }
            set
            {
                this.tnc = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string CoverCostText
        {
            get
            {
                return this.coverCostText;
            }
            set
            {
                this.coverCostText = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string CoverCost
        {
            get
            {
                return this.coverCost;
            }
            set
            {
                this.coverCost = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string CoverCostStatus
        {
            get
            {
                return this.coverCostStatus;
            }
            set
            {
                this.coverCostStatus = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string Img
        {
            get
            {
                return this.img;
            }
            set
            {
                this.img = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string ButtonTextInProdPage
        {
            get
            {
                return this.buttonTextInProdPage;
            }
            set
            {
                this.buttonTextInProdPage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string ButtonTextInRTIPage
        {
            get
            {
                return this.buttonTextInRTIPage;
            }
            set
            {
                this.buttonTextInRTIPage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public bool IsRegistered
        {
            get
            {
                return this.isRegistered;
            }
            set
            {
                this.isRegistered = value;
            }
        }
        public string LegalInformation
        {
            get
            {
                return this.legalInformation;
            }
            set
            {
                this.legalInformation = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string LegalInformationText
        {
            get
            {
                return this.legalInformationText;
            }
            set
            {
                this.legalInformationText = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string PopUpMessage
        {
            get
            {
                return this.popUpMessage;
            }
            set
            {
                this.popUpMessage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public double OldAmount
        {
            get
            {
                return this.oldAmount;
            }
            set
            {
                this.oldAmount = value;
            }
        }
        public string OldQuoteId
        {
            get
            {
                return this.oldQuoteId;
            }
            set
            {
                this.oldQuoteId = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string TncSecondaryFOPPage
        {
            get
            {
                return this.tncSecondaryFOPPage;
            }
            set
            {
                this.tncSecondaryFOPPage = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string PaymentContent
        {
            get
            {
                return this.paymentContent;
            }
            set
            {
                this.paymentContent = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string PaymentContentHeader
        {
            get
            {
                return this.paymentContentHeader;
            }
            set
            {
                this.paymentContentHeader = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string PaymentContentBody
        {
            get
            {
                return this.paymentContentBody;
            }
            set
            {
                this.paymentContentBody = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string DisplayAmount
        {
            get
            {
                return this.displayAmount;
            }
            set
            {
                this.displayAmount = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public string ConfirmationMsg
        {
            get
            {
                return this.confirmationMsg;
            }
            set
            {
                this.confirmationMsg = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
        public string ConfirmationEmailForTPIPurcahse
        {
            get
            {
                return this.confirmationEmailForTPIPurcahse;
            }
            set
            {
                this.confirmationEmailForTPIPurcahse = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }

        public bool IsTPIIncludedInCart
        {
            get
            {
                return this.isTPIIncludedInCart;
            }
            set
            {
                this.isTPIIncludedInCart = value;
            }
        }

        public MOBItem PolicyOfInsuranceTextAndUrl
        {
            get { return this.policyOfInsuranceTextAndUrl; }
            set { this.policyOfInsuranceTextAndUrl = value; }
        }

        public MOBItem TermsAndConditionsTextAndUrl
        {
            get { return this.termsAndConditionsTextAndUrl; }
            set { this.termsAndConditionsTextAndUrl = value; }
        }


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
