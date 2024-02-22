using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using United.Service.Presentation.CommonModel;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class MealOfferResponse
    {
        private List<PreSelectedMeal> preSelectedMeals;

        public List<PreSelectedMeal> PreSelectedMeals
        {
            get
            {
                if (preSelectedMeals == null)
                {
                    preSelectedMeals = new List<PreSelectedMeal>();
                }
                return preSelectedMeals;
            }
            set { preSelectedMeals = value; }
        }

        private List<InEligibleSegment> inEligibleSegments;

        public List<InEligibleSegment> InEligibleSegments
        {
            get { return inEligibleSegments; }
            set { inEligibleSegments = value; }
        }

        private List<AvailableMeal> availableMeals;

        public List<AvailableMeal> AvailableMeals
        {
            get { return availableMeals; }
            set { availableMeals = value; }
        }

        private ServiceResponse errorResponse;

        public ServiceResponse ErrorResponse
        {
            get { return errorResponse; }
            set { errorResponse = value; }
        }

    }

    [Serializable]
    public class PreSelectedMeal : BaseMeal
    {
        private int segmentNumber;

        public int SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }

        private string passengerSharePosition;

        public string PassengerSharePosition
        {
            get { return passengerSharePosition; }
            set { passengerSharePosition = value; }
        }

        private string orderId;
        public string OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }
        private string imageUrl;
        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

    }

    [Serializable]
    public class AvailableMeal : BaseMeal
    {
        private string imageUrl;

        public string ImageUrl
        {
            get { return imageUrl; }
            set { imageUrl = value; }
        }

        private List<int> segmentNumbers;
        public List<int> SegmentNumbers
        {
            get { return segmentNumbers; }
            set { segmentNumbers = value; }
        }
        private string specialMealTitle;

        public string SpecialMealTitle
        {
            get { return specialMealTitle; }
            set { specialMealTitle = value; }
        }

        private List<BaseMeal> specialMeals;

        public List<BaseMeal> SpecialMeals
        {
            get { return specialMeals; }
            set { specialMeals = value; }
        }
    }

    [Serializable]
    public class InEligibleSegment
    {
        private int segmentNumber;
        public int SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }

        private string reasonCode;
        public string ReasonCode
        {
            get { return reasonCode; }
            set { reasonCode = value; }
        }

        private string reasonDescription;
        public string ReasonDescription
        {
            get { return reasonDescription; }
            set { reasonDescription = value; }
        }
    }

    [Serializable]
    public class BaseMeal : GeneralBaseMeal
    {

        private string mealServiceCode;
        public string MealServiceCode
        {
            get { return mealServiceCode; }
            set { mealServiceCode = value; }
        }

        private int mealCount;

        public int MealCount
        {
            get { return mealCount; }
            set { mealCount = value; }
        }
        private bool isMealAvailable;

        public bool IsMealAvailable
        {
            get { return isMealAvailable; }
            set { isMealAvailable = value; }
        }

        private int mealSourceType;

        public int MealSourceType
        {
            get { return mealSourceType; }
            set { mealSourceType = value; }
        }


        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public string errorCode;
        public string ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        private int offerQuantity;

        public int OfferQuantity
        {
            get { return offerQuantity; }
            set { offerQuantity = value; }
        }

        private string offerType;

        public string OfferType
        {
            get { return offerType; }
            set { offerType = value; }
        }
    }

    [Serializable]
    public class GeneralBaseMeal
    {
        private string mealCode;

        public string MealCode
        {
            get { return mealCode; }
            set { mealCode = value; }
        }

        private string mealName;

        public string MealName
        {
            get { return mealName; }
            set { mealName = value; }
        }

        private string mealDescription;

        public string MealDescription
        {
            get { return mealDescription; }
            set { mealDescription = value; }
        }

        private string mealType;

        public string MealType
        {
            get { return mealType; }
            set { mealType = value; }
        }

        private int sequenceNumber;
        public int SequenceNumber
        {
            get { return sequenceNumber; }
            set { sequenceNumber = value; }
        }
    }
    [Serializable]
    public enum MealSourceType
    {
        [Display(Name = "MealOption")]
        Regular = 1,
        [Display(Name = "SpecialMeal")]
        Special = 2,
        [Display(Name = "NonMealOption")]
        NonMeal = 3
    }
}
