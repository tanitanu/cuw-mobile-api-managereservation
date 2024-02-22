using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class TripPlanTile
    {
        public string Header { get; set; }// Title
        public string Body { get; set; } // SubTitle
        public string ImageUrl { get; set; } = null;// ImageUrl
        public string PillText { get; set; } // SubTitle ("New")
        public List<MOBActionButton> ActionButtons { get; set; }

    }
    [Serializable()]
    public class MOBTripPlanOnboarding
    {
        private string header;

        private string body;

        private List<MOBActionButton> actionButtons;

        private List<MOBTripPlanOnboardItem> onboardItems;

        public List<MOBTripPlanOnboardItem> OnboardItems
        {
            get { return onboardItems; }
            set { onboardItems = value; }
        }

        public List<MOBActionButton> ActionButtons
        {
            get { return actionButtons; }
            set { actionButtons = value; }
        }

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Header
        {
            get { return header; }
            set { header = value; }
        }

    }
    [Serializable()]
    public class MOBTripPlanOnboardItem
    {
        private string title;
        private string subTitle;
        private string imageUrl;
        private int order;

        public int Order
        {
            get { return order; }
            set { order = value; }
        }

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

    }
    [Serializable()]
    public class MOBActionButton
    {
        private string actionURL;

        private string actionText;
        private int rank;
        private bool isPrimary;
        private bool isEnabled;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsPrimary
        {
            get { return isPrimary; }
            set { isPrimary = value; }
        }

        public int Rank
        {
            get { return rank; }
            set { rank = value; }
        }

        public string ActionText
        {
            get { return actionText; }
            set { actionText = value; }
        }

        public string ActionURL
        {
            get { return actionURL; }
            set { actionURL = value; }
        }
    }
}
