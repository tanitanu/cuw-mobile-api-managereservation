using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBOARecordLocator
    {
        private string carrierCode = string.Empty;
        private string carrierName = string.Empty;
        private string recordLocator = string.Empty;
        private string createDate = string.Empty;

        public string CarrierCode
        {
            get
            {
                return this.carrierCode;
            }
            set
            {
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string CarrierName
        {
            get
            {
                return this.carrierName;
            }
            set
            {
                this.carrierName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string CreateDate
        {
            get
            {
                return this.createDate;
            }
            set
            {
                this.createDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
