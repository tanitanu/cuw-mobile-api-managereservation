using System;

namespace United.Mobile.Model.FlightStatus
{
    public class PreBoardActionItem
    {
        public string ActionIcon { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Details { get; set; }

        public string DetailsImage { get; set; }

        public bool DetailsExpanded { get; set; }

        public bool Complete { get; set; }

        public DateTime LastUpdatedDateTime { get; set; }

        public int DefaultPriority { get; set; }

        public string StatusText { get; set; }

        public string ArrivalTimeText { get; set; }

        public string TransitionActionIcon { get; set; }
    }
}
