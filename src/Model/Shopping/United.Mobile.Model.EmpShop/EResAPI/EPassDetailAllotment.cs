using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    public class EPassDetailAllotment
    {
        /// <summary>
        /// Gets or sets the EffectiveDate
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the ExpirationDate
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the Flown
        /// </summary>
        public int Flown { get; set; }

        /// <summary>
        /// Gets or sets the Pending
        /// </summary>
        public int Pending { get; set; }

        /// <summary>
        /// Gets or sets the Remaining
        /// </summary>
        public int Remaining { get; set; }

        /// <summary>
        /// Gets or sets the ProgramYear
        /// </summary>
        public string ProgramYear { get; set; } = null;

        /// <summary>
        /// Gets or sets the TotalCount
        /// </summary>
        public int TotalCount { get; set; }
    }
}
