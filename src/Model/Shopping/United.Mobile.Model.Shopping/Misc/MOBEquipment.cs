using System;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Equipment
    {
        private string id = string.Empty;
        private string noseNumber;
        private string tailNumber;
        private Aircraft aircraft;
        private int cabinCount = 1;

        public int CabinCount
        {
            get { return cabinCount; }
            set { cabinCount = value; }
        }



        public Equipment()
        {
        }

        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string NoseNumber
        {
            get
            {
                return this.noseNumber;
            }
            set
            {
                this.noseNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TailNumber
        {
            get
            {
                return this.tailNumber;
            }
            set
            {
                this.tailNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public Aircraft Aircraft
        {
            get
            {
                return this.aircraft;
            }
            set
            {
                this.aircraft = value;
            }
        }
    }
}
