using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Pcu
{
    [Serializable()]
    public class PcuPurchaseConfirmation
    {
        private List<PcuSegment> purchasedSegments;
        private List<MOBItem> captions;
        private string formattedTotalPrice;
        private string formattedTotalRefundPrice;
        private string totalWithCreditCardInfo;
        private string emailAddress;
        private string recordLocator;
        private string lastName;
        private string cartId;
        private string creditCardDisplayNumber;
        private string cardTypeDescription;

        public List<PcuSegment> PurchasedSegments
        {
            get { return purchasedSegments; }
            set { purchasedSegments = value; }
        }

        public List<MOBItem> Captions
        {
            get { return captions; }
            set { captions = value; }
        }

        public string FormattedTotalPrice
        {
            get { return formattedTotalPrice; }
            set { formattedTotalPrice = value; }
        }

        public string FormattedTotalRefundPrice
        {
            get { return formattedTotalRefundPrice; }
            set { formattedTotalRefundPrice = value; }
        }

        public string TotalWithCreditCardInfo
        {
            get { return totalWithCreditCardInfo; }
            set { totalWithCreditCardInfo = value; }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public string RecordLocator
        {
            get { return recordLocator; }
            set { recordLocator = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        public string CreditCardDisplayNumber
        {
            get { return creditCardDisplayNumber; }
            set { creditCardDisplayNumber = value; }
        }

        public string CardTypeDescription
        {
            get { return cardTypeDescription; }
            set { cardTypeDescription = value; }
        }

    }
}
