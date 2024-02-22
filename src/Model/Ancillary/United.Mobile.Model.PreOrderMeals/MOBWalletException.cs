using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class MOBWalletException
    {
        private string code = string.Empty;
        private string message = string.Empty;
        private List<string> messageList;

        public MOBWalletException()
        {
        }

        public MOBWalletException(string code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<string> MessageList
        {

            get
            {

                return this.messageList;

            }

            set
            {

                this.messageList = value;

            }

        }

    }
}
