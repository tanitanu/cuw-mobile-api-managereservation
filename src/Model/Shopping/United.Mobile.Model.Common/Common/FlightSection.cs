using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightSection
    {
        private string sectionName = string.Empty;
        private decimal priceFrom;
        private List<MOBSHOPFlattenedFlight> flattenedFlights;

        public string SectionName
        {
            get
            {
                return this.sectionName;
            }
            set
            {
                this.sectionName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public decimal PriceFrom
        {
            get
            {
                return this.priceFrom;
            }
            set
            {
                this.priceFrom = value;
            }
        }

        public List<MOBSHOPFlattenedFlight> FlattenedFlights
        {
            get
            {
                return this.flattenedFlights;
            }
            set
            {
                this.flattenedFlights = value;
            }
        }
        public FlightSection()
        {
            FlattenedFlights = new List<MOBSHOPFlattenedFlight>();
        }
    }
}
