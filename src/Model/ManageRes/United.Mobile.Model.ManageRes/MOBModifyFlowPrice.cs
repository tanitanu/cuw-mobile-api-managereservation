using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBModifyFlowPrice
    {
        private string description;
        private string formattedValue;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string FormattedValue
        {
            get { return formattedValue; }
            set { formattedValue = value; }
        }
    }
}
