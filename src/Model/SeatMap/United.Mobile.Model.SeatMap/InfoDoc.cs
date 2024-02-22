using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class InfoDoc
    {
        public string Base64AztecCode { get; set; }
        public string ButtonLabel { get; set; }
        public List<Dictionary<string, string>> Captions { get; set; }
        public string Pnr { get; set; }
        public string Title { get; set; }
    }
}
