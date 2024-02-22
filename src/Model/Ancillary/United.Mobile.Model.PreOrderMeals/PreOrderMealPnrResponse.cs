using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.ReShop;
using United.Service.Presentation.CommonModel;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable()]
    public class PreOrderMealPnrResponse : MOBPNRByRecordLocatorResponse
    {
        private List<FlightSegmentSpecialMeal> flightSegmentSpecialMeals;

        public List<FlightSegmentSpecialMeal> FlightSegmentSpecialMeals
        {
            get
            {
                if (flightSegmentSpecialMeals == null)
                {
                    flightSegmentSpecialMeals = new List<FlightSegmentSpecialMeal>();
                }

                return flightSegmentSpecialMeals;
            }
            set { flightSegmentSpecialMeals = value; }
        }

        private List<Amenity> allSpecialMeals;

        public List<Amenity> AllSpecialMeals
        {
            get
            {
                if (allSpecialMeals == null)
                {
                    allSpecialMeals = new List<Amenity>();
                }
                return allSpecialMeals;
            }
            set
            {
                allSpecialMeals = value;
            }
        }
    }
}
