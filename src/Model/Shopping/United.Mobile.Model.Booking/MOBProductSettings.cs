using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Booking
{
    public class MOBProductSettings
    {
            public List<MOBProductElements> Products { get; set; }

        
    }
    public class MOBProductElements
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; }
        public string CabinCount { get; set; }
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Details { get; set; } 
        public string ShouldShowShortCabinName { get; set; } = string.Empty;

    }
}
