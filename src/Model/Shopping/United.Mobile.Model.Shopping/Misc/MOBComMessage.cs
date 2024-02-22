using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ComMessage
    {
        private string code = string.Empty;
        private int displaySequence;
        private string key = string.Empty;
        private string text = string.Empty;

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

        public int DisplaySequence
        {
            get
            {
                return this.displaySequence;
            }
            set
            {
                this.displaySequence = value;
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
