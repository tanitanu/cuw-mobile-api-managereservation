using System;

namespace United.Mobile.Model
{
    [Serializable]
    public class MOBVersion
    {

        public string DisplayText { get; set; } 

        public string Major { get; set; } = string.Empty;

        public string Minor { get; set; } = string.Empty;

        public string Build { get; set; }
    }
}