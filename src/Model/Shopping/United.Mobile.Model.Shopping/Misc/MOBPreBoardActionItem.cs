using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBPreBoardActionItem
    {
        private string actionIcon;
        private string title;
        private string description;
        private string details;
        private string detailsImage;
        private bool detailsExpanded;
        private bool complete;
        private DateTime lastUpdatedDateTime;
        private int defaultPriority;
        private string statusText;
        private string arrivalTimeText;
        private string transitionActionIcon;

        public string ActionIcon
        {
            get
            {
                return this.actionIcon;
            }
            set
            {
                this.actionIcon = value;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
            }
        }

        public string Details
        {
            get
            {
                return this.details;
            }
            set
            {
                this.details = value;
            }
        }

        public string DetailsImage
        {
            get
            {
                return this.detailsImage;
            }
            set
            {
                this.detailsImage = value;
            }
        }

        public bool DetailsExpanded
        {
            get
            {
                return this.detailsExpanded;
            }
            set
            {
                this.detailsExpanded = value;
            }
        }

        public bool Complete
        {
            get
            {
                return this.complete;
            }
            set
            {
                this.complete = value;
            }
        }

        public DateTime LastUpdatedDateTime
        {
            get
            {
                return this.lastUpdatedDateTime;
            }
            set
            {
                this.lastUpdatedDateTime = value;
            }
        }

        public int DefaultPriority
        {
            get
            {
                return this.defaultPriority;
            }
            set
            {
                this.defaultPriority = value;
            }
        }

        public string StatusText
        {
            get
            {
                return this.statusText;
            }
            set
            {
                this.statusText = value;
            }
        }

        public string ArrivalTimeText
        {
            get
            {
                return this.arrivalTimeText;
            }
            set
            {
                this.arrivalTimeText = value;
            }
        }

        public string TransitionActionIcon
        {
            get
            {
                return this.transitionActionIcon;
            }
            set
            {
                this.transitionActionIcon = value;
            }
        }
    }
}
