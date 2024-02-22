using System;
using System.Web;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBMobileCMSContentMessages
    {
        private string contentFull = string.Empty;
        private string contentShort = string.Empty;
        private string headLine = string.Empty;
        private string locationCode = string.Empty;
        private string title = string.Empty;

        public string ContentFull
        {
            get
            {
                return HttpUtility.HtmlDecode(contentFull);
            }
            set
            {
                this.contentFull = HttpUtility.HtmlDecode(string.IsNullOrEmpty(value) ? string.Empty : value.Trim());
            }
        }

        public string ContentShort
        {
            get
            {
                return this.contentShort;
            }
            set
            {
                this.contentShort = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string HeadLine
        {
            get
            {
                return headLine;
            }
            set
            {
                this.headLine = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LocationCode
        {
            get
            {
                return locationCode;
            }
            set
            {
                this.locationCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                this.title = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
