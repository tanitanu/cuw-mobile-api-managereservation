using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPStatementDate
    {
        private string startDate = string.Empty;
        private string endDate = string.Empty;

        public string StartDate
        {
            get
            {
                return this.startDate;
            }
            set
            {
                this.startDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EndDate
        {
            get
            {
                return this.endDate;
            }
            set
            {
                this.endDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
