using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.ManageRes
{
    [Serializable]
    public class MOBTravelSpecialNeeds
    {
        private List<MOBTravelSpecialNeed> specialMeals;
        private List<MOBTravelSpecialNeed> specialRequests;
        private List<MOBTravelSpecialNeed> serviceAnimals;
        private List<MOBItem> specialMealsMessages;
        private List<MOBItem> specialRequestsMessages;
        private List<MOBItem> serviceAnimalsMessages;
        private List<MOBTravelSpecialNeed> highTouchItems;
        private string mealUnavailable;
        private string accommodationsUnavailable;
        private MOBAlertMessages specialNeedsAlertMessages;
        public MOBAlertMessages SpecialNeedsAlertMessages
        {
            get { return specialNeedsAlertMessages; }
            set { specialNeedsAlertMessages = value; }
        }

        public string MealUnavailable { get { return this.mealUnavailable; } set { this.mealUnavailable = value; } }
        public string AccommodationsUnavailable { get { return this.accommodationsUnavailable; } set { this.accommodationsUnavailable = value; } }
        public List<MOBTravelSpecialNeed> HighTouchItems { get { return this.highTouchItems; } set { this.highTouchItems = value; } }
        public List<MOBTravelSpecialNeed> SpecialMeals
        {
            get { return specialMeals; }
            set { specialMeals = value; }
        }

        public List<MOBItem> SpecialMealsMessages
        {
            get { return specialMealsMessages; }
            set { specialMealsMessages = value; }
        }

        public List<MOBTravelSpecialNeed> SpecialRequests
        {
            get { return specialRequests; }
            set { specialRequests = value; }
        }

        public List<MOBItem> SpecialRequestsMessages
        {
            get { return specialRequestsMessages; }
            set { specialRequestsMessages = value; }
        }

        public List<MOBTravelSpecialNeed> ServiceAnimals
        {
            get { return serviceAnimals; }
            set { serviceAnimals = value; }
        }

        public List<MOBItem> ServiceAnimalsMessages
        {
            get { return serviceAnimalsMessages; }
            set { serviceAnimalsMessages = value; }
        }
    }
}
