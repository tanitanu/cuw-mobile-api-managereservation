using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpTravelType
    {
        private bool isTermsAndConditionsAccepted;
        private int numberOfPassengersInJA;
        private string termsAndConditions = string.Empty;
        private int taxType;
        private string verbiageDescription = string.Empty;

        private List<MOBEmpTravelTypeItem> empTravelTypes;


        public List<MOBEmpTravelTypeItem> EmpTravelTypes
        {
            get
            {
                return empTravelTypes;
            }
            set
            {
                this.empTravelTypes = value;
            }
        }

        public bool IsTermsAndConditionsAccepted
        {
            get
            {
                return isTermsAndConditionsAccepted;
            }
            set
            {
                isTermsAndConditionsAccepted = value;
            }
        }

        public int NumberOfPassengersInJA
        {
            get
            {
                return numberOfPassengersInJA;
            }
            set
            {
                numberOfPassengersInJA = value;
            }
        }

        public string TermsAndConditions
        {
            get
            {
                return this.termsAndConditions;
            }
            set
            {
                this.termsAndConditions = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int TaxType
        {
            get
            {
                return taxType;
            }
            set
            {
                taxType = value;
            }
        }

        public string VerbiageDescription
        {
            get
            {
                return this.verbiageDescription;
            }
            set
            {
                this.verbiageDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
