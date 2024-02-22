using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBInstantElite
    {
        private string consolidatedCode = string.Empty;
        private string effectiveDate = string.Empty;
        private int eliteLevel;
        private int eliteYear;
        private string expirationDate = string.Empty;
        private string promotionCode = string.Empty;



        public int EliteLevel
        {
            get
            {
                return this.eliteLevel;
            }
            set
            {
                this.eliteLevel = value;
            }
        }

        public int EliteYear
        {
            get
            {
                return this.eliteYear;
            }
            set
            {
                this.eliteYear = value;
            }
        }

        public string ConsolidatedCode
        {
            get
            {
                return this.consolidatedCode;
            }
            set
            {
                this.consolidatedCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EffectiveDate
        {
            get
            {
                return this.effectiveDate;
            }
            set
            {
                this.effectiveDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PromotionCode
        {
            get
            {
                return this.promotionCode;
            }
            set
            {
                this.promotionCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
