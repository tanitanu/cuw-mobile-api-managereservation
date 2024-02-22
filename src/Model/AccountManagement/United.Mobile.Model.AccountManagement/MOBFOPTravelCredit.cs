using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
	[Serializable()]
	public  class MOBFOPTravelCredit
    {
        private string pinCode;
        private string yearIssued;
        private double redeemAmount;
        private double initialValue;
        private double currentValue;
        private bool isApplied;
        private bool isRemove = false;
        private string expiryDate;
        private string recordLocator;
        private string creditAmount;
        private double newValueAfterRedeem;
        private string displayRedeemAmount;
        private string displayNewValueAfterRedeem;
        private string promoCode;
		private MOBFOPTravelCredit travelCreditType;
		private string message=string.Empty;
		private List<string> eligibleTravelerNameIndex;
		private string recipient;
        private bool isLookupCredit;
		private List<string> eligibleTravelers;
		 

		public List<string> EligibleTravelers
		{
			get { return eligibleTravelers; }
			set { eligibleTravelers = value; }
		}


		public string Recipient
		{
			get { return recipient; }
			set { recipient = value; }
		}


		public List<string> EligibleTravelerNameIndex
		{
			get { return eligibleTravelerNameIndex; }
			set { eligibleTravelerNameIndex = value; }
		}


		public string Message
		{
			get { return message; }
			set { message = value; }
		}


		public MOBFOPTravelCredit TravelCreditType
		{
			get { return  travelCreditType; }
			set { travelCreditType = value; }
		}

		public string PromoCode
		{
			get { return promoCode; }
			set { promoCode = value; }
		}

		public string CreditAmount
		{
			get { return creditAmount; }
			set { creditAmount = value; }
		}

		public string RecordLocator
		{
			get { return recordLocator; }
			set { recordLocator = value; }
		}

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

		public bool IsApplied
		{
			get { return isApplied; }
			set { isApplied = value; }
		}

		public double NewValueAfterRedeem
		{
			get { return newValueAfterRedeem; }
			set { newValueAfterRedeem = value; }

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

        public bool IsLookupCredit
        {
            get { return isLookupCredit; }
            set { isLookupCredit = value; }
        }
    }
}