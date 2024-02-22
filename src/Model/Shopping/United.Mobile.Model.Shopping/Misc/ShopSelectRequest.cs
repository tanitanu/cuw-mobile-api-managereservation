using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class ShopSelectRequest : MOBRequest
    {
        public string Token { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string CartId { get; set; } = string.Empty;
        public string BBXSolutionSetId { get; set; } = string.Empty;
        public string BBXCellId { get; set; } = string.Empty;
        public bool AwardTravel { get; set; }
    }
}
