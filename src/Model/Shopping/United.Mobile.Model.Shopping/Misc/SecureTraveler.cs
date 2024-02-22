using System;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable()]
    public class SecureTraveler
    {
        public string Key { get; set; } = string.Empty;
        public MOBName Name { get; set; } 
        public string BirthDate { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string RedressNumber { get; set; } = string.Empty;
        public string KnownTravelerNumber { get; set; } = string.Empty;
        public string SequenceNumber { get; set; } = string.Empty;
        public string FlaggedType { get; set; } = string.Empty;
    }
}
