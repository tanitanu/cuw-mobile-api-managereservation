using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.PreOrderMeals
{
    [Serializable]
    public class FlightSegmentSpecialMeal
    {
        private int segmentNumber;

        public int SegmentNumber
        {
            get { return segmentNumber; }
            set { segmentNumber = value; }
        }

        private string tripNumber;

        public string TripNumber
        {
            get { return tripNumber; }
            set { tripNumber = value; }
        }

        private string flightNumber;

        public string FlightNumber
        {
            get { return flightNumber; }
            set { flightNumber = value; }
        }

        private List<MOBTravelSpecialNeed> specialMeals;

        public List<MOBTravelSpecialNeed> SpecialMeals
        {
            get
            {
                if (specialMeals == null)
                {
                    specialMeals = new List<MOBTravelSpecialNeed>();
                }

                return specialMeals;
            }
            set { specialMeals = value; }
        }
    }
}
