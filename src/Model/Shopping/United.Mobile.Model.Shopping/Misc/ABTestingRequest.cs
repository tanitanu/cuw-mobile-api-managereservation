using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ABTestingRequest : MOBRequest
    {
        private string mpAccountNumber = string.Empty;


        public ABTestingRequest()
            : base()
        {
        }

        public string MPAccountNumber
        {
            get
            {
                return this.mpAccountNumber;
            }
            set
            {
                this.mpAccountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
