using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class UITemplateBlockContent
    {
        public string PredictableKey { get; set; }
        public string ContentType { get; set; }
        public Dictionary<string, object> ContentDetails { get; set; }
        public int OrderIndex { get; set; }
    }
}
