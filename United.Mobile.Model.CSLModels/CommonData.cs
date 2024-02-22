using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class CommonData
    {
        /// <summary>
        /// Start date time of a service
        /// </summary>

        public string StartTime { get; set; } = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss:FFF");

        /// <summary>
        /// End date time of a service.
        /// </summary>

        public string EndTime { get; set; }
    }
}
