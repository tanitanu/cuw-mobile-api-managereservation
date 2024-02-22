using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class Passenger : BasePassenger
    {
        private string mealCode = string.Empty;
        private string mealName = string.Empty;
        private bool isMealSelected;

        public string MealCode
        {
            get { return this.mealCode; }
            set { this.mealCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MealName
        {
            get { return this.mealName; }
            set { this.mealName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string mealDescription;

        public string MealDescription
        {
            get { return mealDescription; }
            set { mealDescription = value; }
        }

        public bool IsMealSelected
        {
            get { return isMealSelected; }
            set { isMealSelected = value; }
        }

        private int mealSourceType;

        public int MealSourceType
        {
            get { return mealSourceType; }
            set { mealSourceType = value; }
        }
    }

    [Serializable]
    public class BasePassenger
    {
        private string passengerId = string.Empty;
        private string sharesPosition = string.Empty;
        private string firstName = string.Empty;
        private string middleName = string.Empty;
        private string lastName = string.Empty;
        private string passengerName = string.Empty;

        public string PassengerId
        {
            get { return this.passengerId; }
            set { this.passengerId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string SharesPosition
        {
            get { return this.sharesPosition; }
            set { this.sharesPosition = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string FirstName
        {
            get { return this.firstName; }
            set { this.firstName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string MiddleName
        {
            get { return this.middleName; }
            set { this.middleName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string PassengerName
        {
            get { return this.passengerName; }
            set { this.passengerName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string passengerTypeCode;

        public string PassengerTypeCode
        {
            get { return passengerTypeCode; }
            set { passengerTypeCode = value; }
        }

        private string givenName;

        public string GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }
    }

    [Serializable]
    public class PassengerV2 : BasePassenger
    {
        private List<PassengerMeal> meals;

        public List<PassengerMeal> Meals
        {
            get
            {
                if (meals == null)
                {
                    meals = new List<PassengerMeal>();
                }

                return meals;
            }
            set { meals = value; }
        }

    }

    [Serializable]
    public class PassengerMeal : GeneralBaseMeal
    {
        private bool isMealSelected;

        public bool IsMealSelected
        {
            get { return isMealSelected; }
            set { isMealSelected = value; }
        }

        private int mealSourceType;

        public int MealSourceType
        {
            get { return mealSourceType; }
            set { mealSourceType = value; }
        }

        private string orderId;

        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
    }
}
