using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    public class ExpirationPassDetails
    {
        /// <summary>
        /// Gets or sets the total expiration count
        /// </summary>
        public int ExpirationCount { get; set; } = 0;

        /// <summary>
        /// Gets or sets the expiration year
        /// </summary>
        public DateTime ExpirationYear { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Gets or sets the expiration text
        /// </summary>
        public string ExpirationText { get; set; } = string.Empty;
    }
}
