using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBName
    {
        private string title = string.Empty;
        private string first = string.Empty;
        private string middle = string.Empty;
        private string last = string.Empty;
        private string suffix = string.Empty;
        private string dateOfBirth = string.Empty;
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

        public string First
        {
            get
            {
                return this.first;
            }
            set
            {
                this.first = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Middle
        {
            get
            {
                return this.middle;
            }
            set
            {
                this.middle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Last
        {
            get
            {
                return this.last;
            }
            set
            {
                this.last = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Suffix
        {
            get
            {
                return this.suffix;
            }
            set
            {
                this.suffix = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DateOfBirth
        {
            get
            {
                return this.dateOfBirth;
            }
            set
            {
                this.dateOfBirth = string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
    }
}
