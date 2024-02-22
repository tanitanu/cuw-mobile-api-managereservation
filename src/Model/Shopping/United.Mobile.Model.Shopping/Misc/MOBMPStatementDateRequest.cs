using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MPStatementDateRequest : MOBRequest
    {
        private string mileagePlusNumber = string.Empty;

        public MPStatementDateRequest()
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
    }
}
