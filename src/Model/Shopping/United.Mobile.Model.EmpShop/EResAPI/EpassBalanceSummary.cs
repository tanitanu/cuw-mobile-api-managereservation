using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace United.Mobile.Model.Common
{
    public class EpassBalanceSummary
    {
        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the EffectiveDate
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the ExpirationDate
        /// </summary>
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the Count
        /// </summary>
        public int Count { get; set; } = 0;

        /// <summary>
        /// Gets or sets the Status
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the TravelerType
        /// </summary>
        public string TravelerType { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the TravelerTypeDescription
        /// </summary>
        public string TravelerTypeDescription { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ProgramYear
        /// </summary>
        public int ProgramYear { get; set; }
    }
}
