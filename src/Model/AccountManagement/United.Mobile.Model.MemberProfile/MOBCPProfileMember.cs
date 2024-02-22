using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBCPProfileMember
    {
        public int CustomerId { get; set; }

        public string Key { get; set; } = string.Empty;

        public int ProfileId { get; set; }

        public string LanguageCode { get; set; } = string.Empty;
    }
}
