using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class AirportAdvisoryMessage
    {
        public AirportAdvisoryMessage()
            : base()
        {
        }
        private string buttonTitle;
        private string headerTitle;
        private List<MOBTypeOption> advisoryMessages;

        public string ButtonTitle
        {
            get
            {
                return this.buttonTitle;
            }
            set
            {
                this.buttonTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string HeaderTitle
        {
            get
            {
                return this.headerTitle;
            }
            set
            {
                this.headerTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBTypeOption> AdvisoryMessages
        {
            get
            {
                return this.advisoryMessages;
            }
            set
            {
                this.advisoryMessages = value;
            }
        }
    }
}
