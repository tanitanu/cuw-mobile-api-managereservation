using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    public class CabinProduct
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool CabinCount { get; set; }
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
    }
}
