using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBCPCustomerMetrics
    {

        private string ptcCode = string.Empty;

        public string PTCCode
        {
            get { return ptcCode; }
            set { ptcCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
