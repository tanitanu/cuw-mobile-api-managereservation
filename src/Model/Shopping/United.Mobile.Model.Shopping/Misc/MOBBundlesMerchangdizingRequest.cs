using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class BundlesMerchangdizingRequest : MOBRequest
    {
        private string pnr = string.Empty;

        public string RecordLocator
        {
            get { return this.pnr; }
            set { this.pnr = value; }
        }
    }
}
