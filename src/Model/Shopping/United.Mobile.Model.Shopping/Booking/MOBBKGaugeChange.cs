using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKGaugeChange
    {
        private string destination = string.Empty;
        private string decodedDestination = string.Empty;
        private string equipment = string.Empty;
        private string equipmentDescription = string.Empty;

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DecodedDestination
        {
            get
            {
                return this.decodedDestination;
            }
            set
            {
                this.decodedDestination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Equipment
        {
            get
            {
                return this.equipment;
            }
            set
            {
                this.equipment = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EquipmentDescription
        {
            get
            {
                return this.equipmentDescription;
            }
            set
            {
                this.equipmentDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
