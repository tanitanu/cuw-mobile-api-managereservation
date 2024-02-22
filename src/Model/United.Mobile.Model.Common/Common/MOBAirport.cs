using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBAirport
    {
        private string code = string.Empty;
        private string name = string.Empty;
        private string city = string.Empty;

        public MOBAirport()
        {
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string City
        {
            get
            {
                return this.city;
            }
            set
            {
                this.city = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
