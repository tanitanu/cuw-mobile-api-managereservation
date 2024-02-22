using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBEmpLmxLoyaltyTier
    {
        private string description = string.Empty;
        private string key = string.Empty;
        private int level;
        private List<MOBEmpLmxQuote> lmxQuotes;

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

        public List<MOBEmpLmxQuote> LmxQuotes
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
