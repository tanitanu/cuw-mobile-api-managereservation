using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBFlightStatusShareOption
    {
        private string commonCaption = string.Empty;
        private string url = string.Empty;
        private string placeholderTitle = string.Empty;
        private string emailBody = string.Empty;

        public string CommonCaption
        {
            get
            {
                return this.commonCaption;
            }
            set
            {
                this.commonCaption = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Url
        {
            get
            {
                return this.url;
            }
            set
            {
                this.url = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string PlaceHolderTitle
        {
            get
            {
                return this.placeholderTitle;
            }
            set
            {
                this.placeholderTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string EmailBody
        {
            get
            {
                return this.emailBody;
            }
            set
            {
                this.emailBody = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
