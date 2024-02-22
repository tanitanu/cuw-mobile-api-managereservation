using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBEmpTravelTime
    {
        private string totalMinutes;
        private string hours;
        private string minutes;

        public string TotalMinutes
        {
            get { return this.totalMinutes; }
            set { this.totalMinutes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Hours
        {
            get { return this.hours; }
            set { this.hours = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Minutes
        {
            get { return this.minutes; }
            set { this.minutes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}