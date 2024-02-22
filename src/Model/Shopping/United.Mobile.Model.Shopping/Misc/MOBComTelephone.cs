using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class MOBComTelephone
    {
        private string areaCityCode = string.Empty;
        private string countryAccessCode = string.Empty;
        private string description = string.Empty;
        private int displaySequency;
        private string extension = string.Empty;
        private string phoneNumber = string.Empty;

        public string AreaCityCode
        {
            get
            {
                return this.areaCityCode;
            }
            set
            {
                this.areaCityCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CountryAccessCode
        {
            get
            {
                return this.countryAccessCode;
            }
            set
            {
                this.countryAccessCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int DisplaySequency
        {
            get
            {
                return this.displaySequency;
            }
            set
            {
                this.displaySequency = value;
            }
        }

        public string Extension
        {
            get
            {
                return this.extension;
            }
            set
            {
                this.extension = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PhoneNumber
        {
            get
            {
                return this.phoneNumber;
            }
            set
            {
                this.phoneNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
