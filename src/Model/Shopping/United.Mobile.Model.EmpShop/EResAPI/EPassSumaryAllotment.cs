using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace United.Mobile.Model.Common
{
    public class EPassSumaryAllotment : EpassBalanceSummary
    {

        /// <summary>
        /// Gets or sets the expiration pass details
        /// </summary>
        public ExpirationPassDetails ExpirationPassDetail { get; set; } = new ExpirationPassDetails();

        /// <summary>
        /// Gets or sets the EPassDetailAllotments
        /// </summary>
        public List<EPassDetailAllotment> EPassDetailAllotments { get; set; }
    }
}
