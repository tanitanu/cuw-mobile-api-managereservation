using System;

namespace United.Mobile.Model.Shopping.FormofPayment
{
    [Serializable]
    public class FOPMoneyPlusMiles
    {
        private string optionId = string.Empty;
        private double moneyOwed;
        private double conversionRate;
        private double milesMoneyValue;
        private double milesOwed;
        private double milesPercentage;
        private string milesApplied = string.Empty;
        private string moneyDiscountForMiles = string.Empty;
        private string moneyBalanceDue = string.Empty;
        private string milesRemaining = string.Empty;
        private string reviewMilesApplied = string.Empty;
        private string fare = string.Empty;

        public string OptionId
        {
            get { return optionId; }
            set { this.optionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public double MoneyOwed
        {
            get { return moneyOwed; }
            set { this.moneyOwed = value; }
        }

        public double ConversionRate
        {
            get { return conversionRate; }
            set { this.conversionRate = value; }
        }

        public double MilesMoneyValue
        {
            get { return milesMoneyValue; }
            set { this.milesMoneyValue = value; }
        }

        public double MilesOwed
        {
            get { return milesOwed; }
            set { this.milesOwed = value; }
        }

        public double MilesPercentage
        {
            get { return milesPercentage; }
            set { this.milesPercentage = value; }
        }

        public string MilesApplied
        {
            get { return milesApplied; }
            set { this.milesApplied = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MoneyDiscountForMiles
        {
            get { return moneyDiscountForMiles; }
            set { this.moneyDiscountForMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MoneyBalanceDue
        {
            get { return moneyBalanceDue; }
            set { this.moneyBalanceDue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MilesRemaining
        {
            get { return milesRemaining; }
            set { this.milesRemaining = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ReviewMilesApplied
        {
            get { return reviewMilesApplied; }
            set { this.reviewMilesApplied = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
        public string Fare
        {
            get { return fare; }
            set { this.fare = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }

}
