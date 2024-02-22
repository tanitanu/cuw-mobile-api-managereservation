using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBBKEquipmentDisclosure
    {
        private string equipmentDescription = string.Empty;
        private string equipmentType = string.Empty;
        private bool isSingleCabin;
        private bool noBoardingAssistance;
        private bool nonJetEquipment;
        private bool wheelchairsNotAllowed;

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

        public string EquipmentType
        {
            get
            {
                return this.equipmentType;
            }
            set
            {
                this.equipmentType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsSingleCabin
        {
            get
            {
                return this.isSingleCabin;
            }
            set
            {
                this.isSingleCabin = value;
            }
        }

        public bool NoBoardingAssistance
        {
            get
            {
                return this.noBoardingAssistance;
            }
            set
            {
                this.noBoardingAssistance = value;
            }
        }

        public bool NonJetEquipment
        {
            get
            {
                return this.nonJetEquipment;
            }
            set
            {
                this.nonJetEquipment = value;
            }
        }

        public bool WheelchairsNotAllowed
        {
            get
            {
                return this.wheelchairsNotAllowed;
            }
            set
            {
                this.wheelchairsNotAllowed = value;
            }
        }
    }
}
