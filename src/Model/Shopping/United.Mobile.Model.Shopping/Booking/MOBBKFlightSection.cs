using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKFlightSection
    {
        private string sectionName = string.Empty;
        private decimal priceFrom;
        private List<MOBBKFlattenedFlight> flattenedFlights;

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

        public List<MOBBKFlattenedFlight> FlattenedFlights
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
    }
}
