using System;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBAirline
    {
        private string name = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }


        public string FlightNumber { get; set; } = string.Empty;

    }
}
