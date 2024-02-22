using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlifoScheduleCabin
    {
        public string BoardingTotals = string.Empty;

        public FlifoScheduleCabinMeal[] Meals;

        public string Type = string.Empty;

        ///// <remarks/>
        //[System.Xml.Serialization.XmlElementAttribute(IsNullable = true)]
        //public string BoardingTotals
        //{
        //    get
        //    {
        //        return this.boardingTotals;
        //    }
        //    set
        //    {
        //        this.boardingTotals = value;
        //    }
        //}

        ///// <remarks/>
        //[System.Xml.Serialization.XmlArrayItemAttribute("Meal", IsNullable = false)]
        //public FlifoScheduleCabinMeal[] Meals
        //{
        //    get
        //    {
        //        return this.meals;
        //    }
        //    set
        //    {
        //        this.meals = value;
        //    }
        //}

        ///// <remarks/>
        //public string Type
        //{
        //    get
        //    {
        //        return this.type;
        //    }
        //    set
        //    {
        //        this.type = value;
        //    }
        //}

    }
}
