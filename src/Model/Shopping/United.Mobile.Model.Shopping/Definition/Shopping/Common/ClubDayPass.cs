using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class ClubDayPass
    {
        private string passCode = string.Empty;
        private string mileagePlusNumber = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string clubPassCode = string.Empty;
        private double paymentAmount;
        private string purchaseDate = string.Empty;
        private string expirationDate = string.Empty;
        public string expirationDateTime = string.Empty;


        private byte[] barCode;
        private EnElectronicClubPassesType electronicClubPassesType;

        public string PassCode
        {
            get
            {
                return this.passCode;
            }
            set
            {
                this.passCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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

        public string ClubPassCode
        {
            get
            {
                return this.clubPassCode;
            }
            set
            {
                this.clubPassCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public double PaymentAmount
        {
            get
            {
                return this.paymentAmount;
            }
            set
            {
                this.paymentAmount = value;
            }
        }

        public string PurchaseDate
        {
            get
            {
                return this.purchaseDate;
            }
            set
            {
                this.purchaseDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ExpirationDate
        {
            get
            {
                return this.expirationDate;
            }
            set
            {
                this.expirationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public byte[] BarCode
        {
            get
            {
                return this.barCode;
            }
            set
            {
                this.barCode = value;
            }
        }
        public EnElectronicClubPassesType ElectronicClubPassesType
        {
            get
            {
                return this.electronicClubPassesType;
            }
            set
            {
                this.electronicClubPassesType = value;
            }
        }
        public string ExpirationDateTime
        {
            get { return this.expirationDateTime; }
            set { this.expirationDateTime = value; }
        }
    }

    [Serializable]
    public enum EnElectronicClubPassesType
    {
        PurchasedOTP = 0,
        ChaseCCOTP = 1,
        DefaultOTP = 2
    }

}
