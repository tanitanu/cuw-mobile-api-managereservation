using System.Collections.Generic;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping.Pcu
{
    public class PcuState : PersistBase, IPersist
    {
        #region IPersist Members

        private string objectName = "United.Persist.Definition.Pcu.PcuState";

        public string ObjectName
        {
            get { return objectName; }
            set { objectName = value; }
        }
        #endregion

        private string cartId;
        private string recordLocator;
        private string lastName;
        private int numberOfPax;
        private List<PcuSegment> eligibleSegments;
        private List<string> productsRegistered;
        private bool isPostBookingPurchase;
        private List<string> purchaseSegmentNumbers;
        private bool isPartialSuccess;
        private string cardTypeDescription;
        private string creditCardDisplayNumber;
        private string emailAddress;
        private MOBPremiumCabinUpgrade premiumCabinUpgradeOfferDetail;

        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
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

        public int NumberOfPax
        {
            get { return numberOfPax; }
            set { numberOfPax = value; }
        }

        [XmlArrayItem("MOBPcuSegment")]
        public List<PcuSegment> EligibleSegments
        {
            get { return eligibleSegments; }
            set { eligibleSegments = value; }
        }

        public List<string> ProductsRegistered
        {
            get { return productsRegistered; }
            set { productsRegistered = value; }
        }

        public bool IsPostBookingPurchase
        {
            get { return isPostBookingPurchase; }
            set { isPostBookingPurchase = value; }
        }

        public List<string> PurchaseSegmentNumbers
        {
            get { return purchaseSegmentNumbers; }
            set { purchaseSegmentNumbers = value; }
        }

        public bool IsPartialSuccess
        {
            get { return isPartialSuccess; }
            set { isPartialSuccess = value; }
        }

        public string CardTypeDescription
        {
            get { return cardTypeDescription; }
            set { cardTypeDescription = value; }
        }
        public string CreditCardDisplayNumber
        {
            get { return creditCardDisplayNumber; }
            set { creditCardDisplayNumber = value; }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; }
        }

        public MOBPremiumCabinUpgrade PremiumCabinUpgradeOfferDetail
        {
            get { return premiumCabinUpgradeOfferDetail; }
            set { premiumCabinUpgradeOfferDetail = value; }
        }
    }

}
