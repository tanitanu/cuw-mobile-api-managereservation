using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class MealsDetailResponse : MOBResponse
    {
        private List<AvailableMeal> availableMeals;

        public List<AvailableMeal> AvailableMeals
        {
            get
            {
                if (availableMeals == null)
                {
                    availableMeals = new List<AvailableMeal>();
                }

                return availableMeals;
            }
            set { availableMeals = value; }
        }

        private string footerDescription;

        public string FooterDescription
        {
            get { return footerDescription; }
            set { footerDescription = value; }
        }

        private string otherMealOptionTitle;

        public string OtherMealOptionTitle
        {
            get { return otherMealOptionTitle; }
            set { otherMealOptionTitle = value; }
        }

        private string learnMoreAboutSpeciaMeal;

        public string LearnMoreAboutSpeciaMeal
        {
            get { return learnMoreAboutSpeciaMeal; }
            set { learnMoreAboutSpeciaMeal = value; }
        }

        private string specialMealNote;

        public string SpecialMealNote
        {
            get { return specialMealNote; }
            set { specialMealNote = value; }
        }

        private string learnAboutPolarisLounge;

        public string LearnAboutPolarisLounge
        {
            get { return learnAboutPolarisLounge; }
            set { learnAboutPolarisLounge = value; }
        }

        private List<BaseMeal> otherMealOptions;

        public List<BaseMeal> OtherMealOptions
        {
            get
            {
                if (otherMealOptions == null)
                {
                    otherMealOptions = new List<BaseMeal>();
                }

                return otherMealOptions;
            }
            set { otherMealOptions = value; }
        }
    }
}
