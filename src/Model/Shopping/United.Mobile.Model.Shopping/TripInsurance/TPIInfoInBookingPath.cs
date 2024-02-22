using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Shopping.TripInsurance
{
    [Serializable]
    public class TPIInfoInBookingPath
    {
        public List<MOBItem> TPIAIGReturnedMessageContentList { get; set; }
        public string QuoteId { get; set; }
        public double Amount { get; set; }
        public string Title { get; set; } = string.Empty;

        public string Header { get; set; } = string.Empty;

        public List<string> Content { get; set; }
        public string Tnc { get; set; } = string.Empty;

        public string CoverCostText { get; set; } = string.Empty;

        public string CoverCost { get; set; } = string.Empty;

        public string CoverCostStatus { get; set; } = string.Empty;

        public string Img { get; set; } = string.Empty;

        public string ButtonTextInProdPage { get; set; } = string.Empty;

        public string ButtonTextInRTIPage { get; set; } = string.Empty;

        public bool IsRegistered { get; set; }

        public string LegalInformation { get; set; } = string.Empty;

        public string LegalInformationText { get; set; } = string.Empty;

        public string PopUpMessage { get; set; } = string.Empty;

        public double OldAmount { get; set; }
        public string OldQuoteId { get; set; } = string.Empty;

        public string TncSecondaryFOPPage { get; set; } = string.Empty;

        public string PaymentContent { get; set; } = string.Empty;

        public string PaymentContentHeader { get; set; } = string.Empty;

        public string PaymentContentBody { get; set; } = string.Empty;

        public string DisplayAmount { get; set; } = string.Empty;

        public string ConfirmationMsg { get; set; } = string.Empty;

        public string ConfirmationEmailForTPIPurcahse { get; set; } = string.Empty;


        public bool IsTPIIncludedInCart { get; set; }
        public TPIInfoInBookingPath()
        {
            TPIAIGReturnedMessageContentList = new List<MOBItem>();
            Content = new List<string>();
        }
    }
}
