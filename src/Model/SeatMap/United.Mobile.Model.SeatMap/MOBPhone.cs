using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBPhone
    {
        private string key = string.Empty;
        private MOBChannel channel;
        private MOBCountry country;
        private bool isPrivate;
        private bool isDefault;
        private bool isPrimary;
        private bool isProfileOwner;
        private string phoneNumber = string.Empty;
        private string areaNumber = string.Empty;
        private string phoneNumberDisclaimer = "";//TODO:ConfigurationManager.AppSettings["PhoneNumberDisclaimer"];
        private string extensionNumber = string.Empty;
        private bool isThisPhoneFromProfileOwner;

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public MOBChannel Channel
        {
            get
            {
                return this.channel;
            }
            set
            {
                this.channel = value;
            }
        }

        public MOBCountry Country
        {
            get
            {
                return this.country;
            }
            set
            {
                this.country = value;
            }
        }

        public bool IsPrivate
        {
            get
            {
                return this.isPrivate;
            }
            set
            {
                this.isPrivate = value;
            }
        }

        public bool IsDefault
        {
            get
            {
                return this.isDefault;
            }
            set
            {
                this.isDefault = value;
            }
        }

        public bool IsPrimary
        {
            get
            {
                return this.isPrimary;
            }
            set
            {
                this.isPrimary = value;
            }
        }

        public bool IsProfileOwner
        {
            get
            {
                return this.isProfileOwner;
            }
            set
            {
                this.isProfileOwner = value;
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

        public string AreaNumber
        {
            get
            {
                return this.areaNumber;
            }
            set
            {
                this.areaNumber = value;
            }
        }

        public string PhoneNumberDisclaimer
        {
            get
            {
                return this.phoneNumberDisclaimer;
            }
            set
            {

            }
        }

        public string ExtensionNumber
        {
            get
            {
                return this.extensionNumber;
            }
            set
            {
                this.extensionNumber = value;
            }
        }

        public bool ISThisPhoneFromProfileOwner
        {
            get
            {
                return this.isThisPhoneFromProfileOwner;
            }
            set
            {
                this.isThisPhoneFromProfileOwner = value;
            }
        }
    }
}
