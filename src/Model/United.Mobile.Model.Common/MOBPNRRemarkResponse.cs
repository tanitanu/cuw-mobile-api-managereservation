using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBPNRRemarkResponse : MOBResponse
    {
        private string recordLocator = string.Empty;

        public MOBPNRRemarkResponse()
            : base()
        {
        }

        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
