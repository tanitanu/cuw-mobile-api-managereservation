using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable()]
    public class MOBSHOPFareLock
    {
        private string fareLockHoldButtonText = string.Empty;
        public string FareLockHoldButtonText
        {
            get { return fareLockHoldButtonText; }
            set { fareLockHoldButtonText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareLockTextTop = string.Empty;
        public string FareLockTextTop
        {
            get { return fareLockTextTop; }
            set { fareLockTextTop = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareLockTextBottom = string.Empty;
        public string FareLockTextBottom
        {
            get { return fareLockTextBottom; }
            set { fareLockTextBottom = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareLockPurchaseButtonText = string.Empty;
        public string FareLockPurchaseButtonText
        {
            get { return fareLockPurchaseButtonText; }
            set { fareLockPurchaseButtonText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private double fareLockPurchaseButtonAmount = 0.0;
        public double FareLockPurchaseButtonAmount
        {
            get { return fareLockPurchaseButtonAmount; }
            set { fareLockPurchaseButtonAmount = value; }
        }

        private string fareLockPurchaseButtonAmountDisplayText = string.Empty;
        public string FareLockPurchaseButtonAmountDisplayText
        {
            get { return fareLockPurchaseButtonAmountDisplayText; }
            set { fareLockPurchaseButtonAmountDisplayText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private double fareLockMinAmount = 0.0;
        public double FareLockMinAmount
        {
            get { return fareLockMinAmount; }
            set { fareLockMinAmount = value; }
        }

        private string fareLockDisplayMinAmount = string.Empty;
        public string FareLockDisplayMinAmount
        {
            get { return fareLockDisplayMinAmount; }
            set { fareLockDisplayMinAmount = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareLockTitleText = string.Empty;
        public string FareLockTitleText
        {
            get { return fareLockTitleText; }
            set { fareLockTitleText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareLockDescriptionText = string.Empty;
        public string FareLockDescriptionText
        {
            get { return fareLockDescriptionText; }
            set { fareLockDescriptionText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string fareLockDisclaimerText = string.Empty;
        public string FareLockDisclaimerText
        {
            get { return fareLockDisclaimerText; }
            set { fareLockDisclaimerText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private List<string> fareLockTermsAndConditions;
        public List<string> FareLockTermsAndConditions
        {
            get { return fareLockTermsAndConditions; }
            set { fareLockTermsAndConditions = value; }
        }

        private List<MOBSHOPFareLockProduct> fareLockProducts;
        public List<MOBSHOPFareLockProduct> FareLockProducts
        {
            get { return fareLockProducts; }
            set { fareLockProducts = value; }
        }
    }
}
