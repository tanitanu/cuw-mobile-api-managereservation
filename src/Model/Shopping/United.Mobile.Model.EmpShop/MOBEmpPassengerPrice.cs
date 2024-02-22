using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpPassengerPrice
    {
        private string baseFare;
        private string destination;
        private List<string> errors;
        private decimal rawBaseFare;
        private List<MOBEmpTax> taxes;
        private string totalFare;
        private decimal totalFareRaw;

        public string BaseFare 
        {
            get
            {
                return baseFare;
            }
            set
            {
                this.baseFare = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public List<string> Errors
        {
            get
            {
                return errors;
            }
            set
            {
                this.errors = value;
            }
        }
        public decimal RawBaseFare
        {
            get
            {
                return rawBaseFare;
            }
            set
            {
                this.rawBaseFare = value;
            }
        }
        public List<MOBEmpTax> Taxes
        {
            get
            {
                return taxes;
            }
            set
            {
                this.taxes = value;
            }
        }
        public string TotalFare
        {
            get
            {
                return totalFare;
            }
            set
            {
                this.totalFare = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public decimal TotalFareRaw 
        {
            get
            {
                return totalFareRaw;
            }
            set
            {
                this.totalFareRaw = value;
            }
        }
    }
}
