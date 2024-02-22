using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPStatementRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;
        private string startDate = string.Empty;
        private string endDate = string.Empty;

        public MPStatementRequest()
            : base()
        {
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }


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
