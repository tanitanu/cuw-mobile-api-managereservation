using System;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBWalletWeather
    {
        private string airportCode = string.Empty;
        private string airportName = string.Empty;
        private string tempF= string.Empty;
        private string tempC = string.Empty;
        private string weatherCondition = string.Empty;
        private string icon = string.Empty;
        private int sky;
        private string lastUpdated = string.Empty;

        public string AirportCode
        {
            get
            {
                return this.airportCode;
            }
            set
            {
                this.airportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string AirportName
        {
            get
            {
                return this.airportName;
            }
            set
            {
                this.airportName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string TempF
        {
            get
            {
                return this.tempF;
            }
            set
            {
                this.tempF = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TempC
        {
            get
            {
                return this.tempC;
            }
            set
            {
                this.tempC = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string WeatherCondition
        {
            get
            {
                return this.weatherCondition;
            }
            set
            {
                this.weatherCondition = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Icon
        {
            get
            {
                return this.icon;
            }
            set
            {
                this.icon = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int Sky
        {
            get
            {
                return this.sky;
            }
            set
            {
                this.sky = value;
            }
        }

        public string LastUpdated
        {
            get
            {
                return this.lastUpdated;
            }
            set
            {
                this.lastUpdated = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
