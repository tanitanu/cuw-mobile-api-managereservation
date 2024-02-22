using System;

namespace United.Mobile.Model.ManagRes
{
    [Serializable()]
    public class MOBAddMPNumberResponse:MOBResponse
    {
        private string mpNumber;
        private MOBName passengerName;

        public string MPNumber
        {
            get
            {
                return this.mpNumber;
            }
            set
            {
                this.mpNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
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
    }
}
