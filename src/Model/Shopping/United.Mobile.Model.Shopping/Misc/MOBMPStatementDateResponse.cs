using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPStatementDateResponse : MOBResponse
    {
        private string mileagePlusNumber = string.Empty;
        private List<MPStatementDate> statementDates;

        public MPStatementDateResponse()
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

        public List<MPStatementDate> StatementDates
        {
            get
            {
                return this.statementDates;
            }
            set
            {
                this.statementDates = value;
            }
        }
    }
}
