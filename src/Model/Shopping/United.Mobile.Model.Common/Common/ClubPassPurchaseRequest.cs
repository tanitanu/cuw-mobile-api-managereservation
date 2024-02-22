using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ClubPassPurchaseRequest
    {

        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private int numberOfPasses;
        private double amountPaid;
        private bool isTestSystem;

        public string FirstName
        {
            get
            {
                return this.firstName;
            }
            set
            {
                this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Email
        {
            get
            {
                return this.email;
            }
            set
            {
                this.email = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string MileagePlusNumber
        {
            get
            {
                return this.mileagePlusNumber;
            }
            set
            {
                this.mileagePlusNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public int NumberOfPasses
        {
            get
            {
                return this.numberOfPasses;
            }
            set
            {
                this.numberOfPasses = value;
            }
        }

        public double AmountPaid
        {
            get
            {
                return this.amountPaid;
            }
            set
            {
                this.amountPaid = value;
            }
        }

        public bool IsTestSystem
        {
            get
            {
                return this.isTestSystem;
            }
            set
            {
                this.isTestSystem = value;
            }
        }

    }
}
