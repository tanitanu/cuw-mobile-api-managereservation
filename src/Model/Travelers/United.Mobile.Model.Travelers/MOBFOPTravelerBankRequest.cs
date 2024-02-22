using System;
using United.Mobile.Model.Common.Shopping;

namespace United.Mobile.Model.Travelers
{
    [Serializable()]
    public class MOBFOPTravelerBankRequest : ShoppingRequest
    {

        private string displayAmount;
        private double appliedAmount;//ewmocvw ti
        private bool isRemove;



        public bool IsRemove
        {
            get { return isRemove; }
            set { isRemove = value; }
        }

        public string DisplayAmount
        {
            get { return displayAmount; }
            set { displayAmount = value; }
        }

        public double AppliedAmount
        {
            get { return appliedAmount; }
            set { appliedAmount = value; }
        }
    }
}
