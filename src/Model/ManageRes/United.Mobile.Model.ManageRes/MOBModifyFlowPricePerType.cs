using System;
using System.Collections.Generic;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBModifyFlowPricePerType
    {
        private string type;
        private string totalBaseFare;
        private string totalTaxesAndFeesPerType;
        private string totalTaxesAndFeesPerPassenger;
        private List<MOBModifyFlowPrice> taxAndFeeBreakdown;
        private int numberOfTravelers;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        public string TotalBaseFare
        {
            get { return totalBaseFare; }
            set { totalBaseFare = value; }
        }

        public string TotalTaxesAndFeesPerType
        {
            get { return totalTaxesAndFeesPerType; }
            set { totalTaxesAndFeesPerType = value; }
        }

        public List<MOBModifyFlowPrice> TaxAndFeeBreakdown
        {
            get { return taxAndFeeBreakdown; }
            set { taxAndFeeBreakdown = value; }
        }

        public int NumberOfTravelers
        {
            get { return numberOfTravelers; }
            set { numberOfTravelers = value; }
        }

        public string TotalTaxesAndFeesPerPassenger
        {
            get { return totalTaxesAndFeesPerPassenger; }
            set { totalTaxesAndFeesPerPassenger = value; }
        }
    }
}