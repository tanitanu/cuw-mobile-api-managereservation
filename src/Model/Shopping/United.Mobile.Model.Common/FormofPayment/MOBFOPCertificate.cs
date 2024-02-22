using System;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable()]
    public class MOBFOPCertificate
    {
        private string pinCode;
        private string yearIssued;
        private string recipientsLastName;
        private string recipientsFirstName;
        private string travelerNameIndex;
        private double redeemAmount;
        private double initialValue;
        private double currentValue;
        private bool isForAllTravelers;
        private double newValueAfterRedeem;
        private string displayRedeemAmount;
        private string displayNewValueAfterRedeem;
        private int index;
        private MOBFOPCertificateTraveler certificateTraveler;
        private bool isCertificateApplied;
        private bool isProfileCertificate;
        private int editingIndex;
        private bool isRemove;
        private string expiryDate;
        //private string promoCode;

        //public string PromoCode
        //{
        //    get { return promoCode; }
        //    set { promoCode = value; }
        //}
        public string ExpiryDate
        {
            get { return expiryDate; }
            set { expiryDate = value; }
        }
        public bool IsRemove
        {
            get { return isRemove; }
            set { isRemove = value; }
        }

        public int EditingIndex
        {
            get { return editingIndex; }
            set { editingIndex = value; }
        }


        public bool IsProfileCertificate
        {
            get { return isProfileCertificate; }
            set { isProfileCertificate = value; }
        }


        public bool IsCertificateApplied
        {
            get { return isCertificateApplied; }
            set { isCertificateApplied = value; }
        }


        public MOBFOPCertificateTraveler CertificateTraveler
        {
            get { return certificateTraveler; }
            set { certificateTraveler = value; }
        }


        public int Index
        {
            get { return index; }
            set { index = value; }
        }
        public double NewValueAfterRedeem
        {
            get { return newValueAfterRedeem; }
            set { newValueAfterRedeem = value; }
        }


        public bool IsForAllTravelers
        {
            get { return isForAllTravelers; }
            set { isForAllTravelers = value; }
        }

        public double RedeemAmount
        {
            get { return redeemAmount; }
            set { redeemAmount = value; }
        }

        public string DisplayRedeemAmount
        {
            get { return displayRedeemAmount; }
            set { displayRedeemAmount = value; }
        }

        public string DisplayNewValueAfterRedeem
        {
            get { return displayNewValueAfterRedeem; }
            set { displayNewValueAfterRedeem = value; }
        }

        public double CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value; }
        }

        public double InitialValue
        {
            get { return initialValue; }
            set { initialValue = value; }
        }


        public string PinCode
        {
            get { return pinCode; }
            set { pinCode = value; }
        }

        public string YearIssued
        {
            get { return yearIssued; }
            set { yearIssued = value; }
        }

        public string RecipientsLastName
        {
            get { return recipientsLastName; }
            set { recipientsLastName = value; }
        }

        public string RecipientsFirstName
        {
            get { return recipientsFirstName; }
            set { recipientsFirstName = value; }
        }

        public string TravelerNameIndex
        {
            get { return travelerNameIndex; }
            set { travelerNameIndex = value; }
        }



    }
}
