using System;
using System.Collections.Generic;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBPriceBreakDown
    {
        private MOBBasePrice basePrice;
        private List<MOBBasePrice> taxes;
        private List<string> passengerTypeCode;
        private int travelerCount;
        private MOBBasePrice totalPrice;
        private MOBBasePrice refundTaxesTotalPrice;
        private MOBBasePrice refundBasePrice;
        private MOBBasePrice refundFee;
        private MOBBasePrice refundFeeEquivalent;
        private MOBBasePrice refundFeePointOfOrigin;

        public int TravelerCount
        {
            get { return travelerCount; }
            set { travelerCount = value; }
        }

        public List<string> PassengerTypeCode
        {
            get { return passengerTypeCode; }
            set { passengerTypeCode = value; }
        }

        public MOBBasePrice BasePrice
        {
            get { return basePrice; }
            set { basePrice = value; }
        }
        public MOBBasePrice RefundFee
        {
            get { return refundFee; }
            set { refundFee = value; }
        }
        public MOBBasePrice TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }
        public MOBBasePrice RefundFeePointOfOrigin
        {
            get { return refundFeePointOfOrigin; }
            set { refundFeePointOfOrigin = value; }
        }

        public MOBBasePrice RefundFeeEquivalent
        {
            get { return refundFeeEquivalent; }
            set { refundFeeEquivalent = value; }
        }

        public MOBBasePrice RefundTaxesTotalPrice
        {
            get { return refundTaxesTotalPrice; }
            set { refundTaxesTotalPrice = value; }
        }

        public MOBBasePrice RefundBasePrice
        {
            get { return refundBasePrice; }
            set { refundBasePrice = value; }
        }
        public List<MOBBasePrice> Taxes
        {
            get { return taxes; }
            set { taxes = value; }
        }

        private List<MOBBasePrice> fees;
        public List<MOBBasePrice> Fees
        {
            get { return fees; }
            set { fees = value; }
        }


    }
}
