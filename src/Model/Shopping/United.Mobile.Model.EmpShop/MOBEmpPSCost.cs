using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpPSCost
    {
        private string ecoLbl;
        private string ecoPrice;
        private string ecoLowPrice;
        private string bfLbl;
        private string bfPrice;
        private string bfLowPrice;
        private string fLbl;
        private string fPrice;
        private string fLowPrice;

        public string EcoLbl
        {
            get
            {
                return this.ecoLbl;
            }
            set
            {
                this.ecoLbl = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EcoPrice
        {
            get
            {
                return this.ecoPrice;
            }
            set
            {
                this.ecoPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string EcoLowPrice
        {
            get
            {
                return this.ecoLowPrice;
            }
            set
            {
                this.ecoLowPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string BfLbl
        {
            get
            {
                return this.bfLbl;
            }
            set
            {
                this.bfLbl = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string BfPrice
        {
            get
            {
                return this.bfPrice;
            }
            set
            {
                this.bfPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string BfLowPrice
        {
            get
            {
                return this.bfLowPrice;
            }
            set
            {
                this.bfLowPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FLbl
        {
            get
            {
                return this.fLbl;
            }
            set
            {
                this.fLbl = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FPrice
        {
            get
            {
                return this.fPrice;
            }
            set
            {
                this.fPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string FLowPrice
        {
            get
            {
                return this.fLowPrice;
            }
            set
            {
                this.fLowPrice = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}