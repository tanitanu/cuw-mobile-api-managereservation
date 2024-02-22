using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.PassRiders
{
    public class DropOption
    {
        public List<TypeOption> Expanded { get; set; }
        public string Id { get; set; }
        public bool Selected { get; set; }
        public string Value { get; set; } 
        public string KeyCode { get; set; }
    }
}
