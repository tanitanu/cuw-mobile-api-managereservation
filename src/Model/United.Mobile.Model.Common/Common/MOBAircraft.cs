using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBAircraft
    {
        public string Code { get; set; } = string.Empty;

        public string ShortName { get; set; } = string.Empty;

        public string LongName { get; set; } = string.Empty;

        public string ModelCode { get; set; } = string.Empty;
    }
}
