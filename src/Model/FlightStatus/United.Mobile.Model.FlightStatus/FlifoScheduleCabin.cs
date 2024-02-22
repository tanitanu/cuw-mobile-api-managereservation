using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable()]
    public class FlifoScheduleCabin
    {
        public string BoardingTotals { get; set; }

        public FlifoScheduleCabinMeal[] Meals;

        public string Type { get; set; }

    }
}
