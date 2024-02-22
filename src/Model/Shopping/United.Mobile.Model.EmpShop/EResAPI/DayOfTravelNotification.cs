using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class DayOfTravelNotification
    {
        /// <summary>
        /// Gets or sets EmailNotification value.
        /// </summary>
        public bool EmailNotification { get; set; } = false;

        /// <summary>
        /// Gets or sets TextNotification value.
        /// </summary>
        public bool TextNotification { get; set; } = false;
    }
}
