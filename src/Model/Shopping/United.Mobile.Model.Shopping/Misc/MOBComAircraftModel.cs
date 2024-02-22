using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ComAircraftModel
    {
        private string description;
        private string genre;
        private string key;
        private string name;
        private string status;
        private string vendorName;

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

        public string Genre
        {
            get
            {
                return this.genre;
            }
            set
            {
                this.genre = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string VendorName
        {
            get
            {
                return this.vendorName;
            }
            set
            {
                this.vendorName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
