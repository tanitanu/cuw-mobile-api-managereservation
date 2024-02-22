using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    [XmlRoot("MOBSHOPSearchFilterItem")]
    [XmlType("MOBSHOPSearchFilterItem")]
    public class MOBSearchFilterItem
    {
        private string key = string.Empty;
        private string value = string.Empty;
        private string displayValue = string.Empty;
        private string amount = string.Empty;
        private string currency = string.Empty;
        private bool isSelected;

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

        public string Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DisplayValue
        {
            get
            {
                return this.displayValue;
            }
            set
            {
                this.displayValue = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public string Amount { get; set; } = string.Empty;

        public string Currency
        {
            get
            {
                return this.currency;
            }
            set
            {
                this.currency = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public bool IsSelected
        {
            get { return this.isSelected; }
            set { this.isSelected = value; }
        }
    }
}
