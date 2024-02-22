using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class LocationEventRequest : MOBRequest
    {
        private List<LocationEventResult> results;

        public List<LocationEventResult> Results
        {
            get
            {
                return this.results;
            }
            set
            {
                this.results = value;
            }
        }
    }
}
