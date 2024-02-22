using System;

namespace United.Mobile.Model.PNRManagement
{
    [Serializable()]
    public class MOBBundle
    {
        private string bundleDescription = string.Empty;
        private string bundle = string.Empty;
        private bool isEPU;
        private bool isPremierAccess;
        private bool isBonusMiles;
        private bool isClubTripPass;
        private bool isExtraBag;
        private string segmentId = string.Empty;
        private string passengerSharesPosition = string.Empty;

        public string PassengerSharesPosition
        {
            get { return this.passengerSharesPosition; }
            set { this.passengerSharesPosition = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SegmentId
        {
            get { return this.segmentId; }
            set { this.segmentId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        //public string Bundle
        //{
        //    get { return this.bundle; }
        //    set { this.bundle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        //}

        public bool IsEPU
        {
            get
            {
                return this.isEPU;
            }
            set
            {
                this.isEPU = value;
            }
        }

        public bool IsPremierAccess
        {
            get
            {
                return this.isPremierAccess;
            }
            set
            {
                this.isPremierAccess = value;
            }
        }

        public bool IsBonusMiles
        {
            get
            {
                return this.isBonusMiles;
            }
            set
            {
                this.isBonusMiles = value;
            }
        }

        public bool IsClubTripPass
        {
            get
            {
                return this.isClubTripPass;
            }
            set
            {
                this.isClubTripPass = value;
            }
        }

        public bool IsExtraBag
        {
            get
            {
                return this.isExtraBag;
            }
            set
            {
                this.isExtraBag = value;
            }
        }

        public string BundleDescription
        {
            get { return this.bundleDescription; }
            set { this.bundleDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
