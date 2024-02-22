using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBLegend
    {
        private string name;
        private string image;
        private string details;

        public MOBLegend()
        {

        }

        public MOBLegend(string name, string image, string details)
        {
            Name = name;
            Image = image;
            Details = details;
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

        public string Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Details
        {
            get
            {
                return this.details;
            }
            set
            {
                this.details = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
