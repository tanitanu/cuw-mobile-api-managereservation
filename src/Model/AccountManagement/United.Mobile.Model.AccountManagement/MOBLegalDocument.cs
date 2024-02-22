using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable]
    public class MOBLegalDocument
    {
        private string title = string.Empty;
        private string document = string.Empty;

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Document
        {
            get
            {
                return this.document;
            }
            set
            {
                this.document = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
