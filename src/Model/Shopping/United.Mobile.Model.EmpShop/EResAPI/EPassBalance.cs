using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    public class EPassBalance
    {
        /// <summary>
        ///  Gets or sets the ExpiringYear
        /// </summary>
        public string ExpiringYear { get; set; }

        /// <summary>
        /// Gets or sets the PassType
        /// </summary>
        public string PassType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the PassTye Descrption
        /// </summary>
        public string PassTypeDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Expire PassCount
        /// </summary>
        public int PassCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the status of the epass
        /// </summary>
        public string Status { get; set; } = string.Empty;

    }
}
