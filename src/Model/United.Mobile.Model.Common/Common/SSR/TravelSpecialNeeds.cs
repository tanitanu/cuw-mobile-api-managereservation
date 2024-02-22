using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common.SSR
{
    [Serializable]
    public class TravelSpecialNeeds
    {
        private List<TravelSpecialNeed> specialMeals=new List<TravelSpecialNeed>();
        private List<TravelSpecialNeed> specialRequests;
        private List<TravelSpecialNeed> serviceAnimals;
        private List<MOBItem> specialMealsMessages;
        private List<MOBItem> specialRequestsMessages;
        private List<MOBItem> serviceAnimalsMessages;
        private List<TravelSpecialNeed> highTouchItems;
        private string mealUnavailable;
        private string accommodationsUnavailable;
        private MOBAlertMessages specialNeedsAlertMessages;
        public MOBAlertMessages SpecialNeedsAlertMessages
        {
            get { return specialNeedsAlertMessages; }
            set { specialNeedsAlertMessages = value; }
        }
        public string MealUnavailable { get { return this.mealUnavailable; } set {this.mealUnavailable = value; } }
        public string AccommodationsUnavailable { get { return this.accommodationsUnavailable; } set { this.accommodationsUnavailable = value; } }

        [XmlArrayItem("MOBTravelSpecialNeed")]
        public List<TravelSpecialNeed> HighTouchItems { get { return this.highTouchItems; } set { this.highTouchItems = value; } }

        public List<TravelSpecialNeed> SpecialMeals
        {
            get { return specialMeals; }
            set
            {
                specialMeals = value ?? new List<TravelSpecialNeed>();
            }
        }

        public List<MOBItem> SpecialMealsMessages
        {
            get { return specialMealsMessages; }
            set { specialMealsMessages = value; }
        }

        [XmlArrayItem("MOBTravelSpecialNeed")]
        public List<TravelSpecialNeed> SpecialRequests
        {
            get { return specialRequests; }
            set { specialRequests = value; }
        }

        public List<MOBItem> SpecialRequestsMessages
        {
            get { return specialRequestsMessages; }
            set { specialRequestsMessages = value; }
        }

        [XmlArrayItem("MOBTravelSpecialNeed")]
        public List<TravelSpecialNeed> ServiceAnimals
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
