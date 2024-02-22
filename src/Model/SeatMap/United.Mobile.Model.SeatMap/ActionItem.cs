using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class ActionItem
    {
        public string Action { get; set; }
        public string ActionText { get; set; }
        public IDictionary<string, string> RequestParameters { get; set; }
    }
}
