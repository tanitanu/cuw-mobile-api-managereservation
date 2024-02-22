using System;

namespace United.Mobile.Model.MPRewards
{
    [Serializable]
    public class MOBPriorityBoardingSelectionResponse : MOBResponse
    {
        private MOBPriorityBoarding pBItemsSelected; // only include PB selected 
        private string pkDispenserPublicKey; // for encryption 
        private double totalPrice;
        private string formattedTotalPrice; // 2 decimal 

        public MOBPriorityBoarding PBItemsSelected
        {
            get { return pBItemsSelected; }
            set { pBItemsSelected = value; }
        }

        public string PkDispenserPublicKey
        {
            get { return pkDispenserPublicKey; }
            set { pkDispenserPublicKey = value; }
        }

        public double TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        public string FormattedTotalPrice
        {
            get { return formattedTotalPrice; }
            set { formattedTotalPrice = value; }
        }

    }
}
