using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping;

namespace United.Persist.Definition.Shopping
{
    [Serializable()]
    public class MOBSHOPFlightSection
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
    }
}
