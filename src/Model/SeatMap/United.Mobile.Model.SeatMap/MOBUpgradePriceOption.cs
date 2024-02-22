using System;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBUpgradePriceOption
    {
        private string value;
        private string type;
        private string code;
        private string rewardCode;
        private string eddCode;

        public string Value { get { return this.value; } set { this.value = value; } }
        public string Type { get { return this.type; } set { this.type = value; } }
        public string Code { get { return this.code; } set { this.code = value; } }
        public string RewardCode { get { return this.rewardCode; } set { this.rewardCode = value; } }
        public string EDDCode { get { return this.eddCode; } set { this.eddCode = value; } }
    }
}
