using System;

namespace United.Mobile.Model.ManagRes
{
    [Serializable()]
    public class MOBAddKTNRedressRequest : MOBRequest
    {
        private string sessionId;
        private string recordLocator ;
        private MOBName passengerName;
        private string knownTravellrNumber;
        private string redressNumber;
        private string key;

        
        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBName PassengerName
        {
            get
            {
                return this.passengerName;
            }
            set
            {
                this.passengerName = value;
            }
        }
        public string KnownTravellrNumber
        {
            get
            {
                return this.knownTravellrNumber; 
            }
            set
            {
                this.knownTravellrNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string RedressNumber
        {
            get
            {
                return this.redressNumber; 
            }
            set
            {
                this.redressNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        public string Key
        {
            get
            {
                return this.key ;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
}
