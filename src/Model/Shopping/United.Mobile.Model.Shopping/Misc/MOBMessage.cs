using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBMessage
    {
        private string codeNumber;
        private string messageCode;
        private string messageText;

        public string CodeNumber
        {
            get { return codeNumber; }
            set
            {
                this.codeNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MessageCode
        {
            get { return messageCode; }
            set
            {
                this.messageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MessageText
        {
            get { return messageText; }
            set
            {
                this.messageText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
