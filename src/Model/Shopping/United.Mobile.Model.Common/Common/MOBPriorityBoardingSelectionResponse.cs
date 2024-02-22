using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBPriorityBoardingSelectionResponse : MOBResponse
    {
        private PriorityBoarding pBItemsSelected; // only include PB selected 
        private string pkDispenserPublicKey; // for encryption 
        private double totalPrice;
        private string formattedTotalPrice; // 2 decimal 

        public PriorityBoarding PBItemsSelected
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
