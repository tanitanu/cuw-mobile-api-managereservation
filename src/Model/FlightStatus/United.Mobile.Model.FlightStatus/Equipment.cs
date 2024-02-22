using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class Equipment
    {
        private string id = string.Empty;
        public int CabinCount { get; set; }
        public string Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
        public string NoseNumber { get; set; }
        public string TailNumber { get; set; }
        public Aircraft Aircraft { get; set; }
        public string Key { get; set; }
    }
}
