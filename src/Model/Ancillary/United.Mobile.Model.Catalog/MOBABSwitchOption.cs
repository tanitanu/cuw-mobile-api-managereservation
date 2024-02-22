using System;

namespace United.Mobile.Model.Catalog
{
    [Serializable]
    public class MOBABSwitchOption
    {
        public string Key { get; set; } = string.Empty;
        public string MPValue { get; set; } = string.Empty;
        public string DefaultValue { get; set; } = string.Empty;
    }
}
