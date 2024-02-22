using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable()]
    public class MOBLmxLoyaltyTier
    {
        private string description = string.Empty;
        private string key = string.Empty;
        private int level;
        private List<MOBLmxQuote> lmxQuotes;

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        public List<MOBLmxQuote> LmxQuotes
        {
            get
            {
                return this.lmxQuotes;
            }
            set
            {
                this.lmxQuotes = value;
            }
        }
    }
}
