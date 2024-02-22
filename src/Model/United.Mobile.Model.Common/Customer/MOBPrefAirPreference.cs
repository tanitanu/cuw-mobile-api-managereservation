using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable]
    public class MOBPrefAirPreference
    {
        private long customerId;
        private long profileId;
        private long airPreferenceId;
        private string key;
        private int mealId;
        private string mealCode;
        private string mealDescription;
        private int seatFrontBack;
        private int seatSide;
        private string seatSideDescription;
        private int numOfFlightsDisplay;
        private int searchPreferenceId;
        private string searchPreferenceDescription;
        private int equipmentId;
        private string equipmentCode;
        private string equipmentDesc;
        private int classId;
        private string classDescription;
        private string airportCode;
        private string airportNameLong = string.Empty;
        private string airportNameShort;
        private int vendorId;
        private string vendorCode;
        private string vendorDescription;
        private bool isNew;
        private bool isSelected;
        private bool isActive;
        private string languageCode;

        private List<United.Mobile.Model.Common.MOBPrefRewardProgram> airRewardPrograms;
        private List<United.Mobile.Model.Common.MOBPrefSpecialRequest> specialRequests;
        private List<MOBPrefServiceAnimal> serviceAnimals;

        public long CustomerId
        {
            get
            {
                return this.customerId;
            }
            set
            {
                this.customerId = value;
            }
        }

        public long ProfileId
        {
            get
            {
                return this.profileId;
            }
            set
            {
                this.profileId = value;
            }
        }

        public long AirPreferenceId
        {
            get
            {
                return this.airPreferenceId;
            }
            set
            {
                this.airPreferenceId = value;
            }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int MealId
        {
            get
            {
                return this.mealId;
            }
            set
            {
                this.mealId = value;
            }
        }

        public string MealCode
        {
            get
            {
                return this.mealCode;
            }
            set
            {
                this.mealCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string MealDescription
        {
            get
            {
                return this.mealDescription;
            }
            set
            {
                this.mealDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int SeatFrontBack
        {
            get
            {
                return this.seatFrontBack;
            }
            set
            {
                this.seatFrontBack = value;
            }
        }

        public int SeatSide
        {
            get
            {
                return this.seatSide;
            }
            set
            {
                this.seatSide = value;
            }
        }

        public string SeatSideDescription
        {
            get
            {
                return this.seatSideDescription;
            }
            set
            {
                this.seatSideDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int NumOfFlightsDisplay
        {
            get
            {
                return this.numOfFlightsDisplay;
            }
            set
            {
                this.numOfFlightsDisplay = value;
            }
        }

        public int SearchPreferenceId
        {
            get
            {
                return this.searchPreferenceId;
            }
            set
            {
                this.searchPreferenceId = value;
            }
        }

        public string SearchPreferenceDescription
        {
            get
            {
                return this.searchPreferenceDescription;
            }
            set
            {
                this.searchPreferenceDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int EquipmentId
        {
            get
            {
                return this.equipmentId;
            }
            set
            {
                this.equipmentId = value;
            }
        }

        public string EquipmentCode { get; set; }

        public string EquipmentDesc
        {
            get
            {
                return this.equipmentDesc;
            }
            set
            {
                this.equipmentDesc = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int ClassId
        {
            get
            {
                return this.classId;
            }
            set
            {
                this.classId = value;
            }
        }

        public string ClassDescription
        {
            get
            {
                return this.classDescription;
            }
            set
            {
                this.classDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AirportCode
        {
            get
            {
                return this.airportCode;
            }
            set
            {
                this.airportCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string AirportNameLong
        {
            get
            {
                return this.airportNameLong;
            }
            set
            {
                this.airportNameLong = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string AirportNameShort
        {
            get
            {
                return this.airportNameShort;
            }
            set
            {
                this.airportNameShort = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int VendorId
        {
            get
            {
                return this.vendorId;
            }
            set
            {
                this.vendorId = value;
            }
        }

        public string VendorCode
        {
            get
            {
                return this.vendorCode;
            }
            set
            {
                this.vendorCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string VendorDescription
        {
            get
            {
                return this.vendorDescription;
            }
            set
            {
                this.vendorDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsNew
        {
            get
            {
                return this.isNew;
            }
            set
            {
                this.isNew = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                this.isSelected = value;
            }
        }

        public bool IsActive
        {
            get
            {
                return this.isActive;
            }
            set
            {
                this.isActive = value;
            }
        }

        public string LanguageCode
        {
            get
            {
                return this.languageCode;
            }
            set
            {
                this.languageCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBPrefRewardProgram> AirRewardPrograms
        {
            get
            {
                return this.airRewardPrograms;
            }
            set
            {
                this.airRewardPrograms = value;
            }
        }

        public List<MOBPrefSpecialRequest> SpecialRequests
        {
            get
            {
                return this.specialRequests;
            }
            set
            {
                this.specialRequests = value;
            }
        }

        public List<MOBPrefServiceAnimal> ServiceAnimals
        {
            get
            {
                return serviceAnimals;
            }
            set
            {
                serviceAnimals = value;
            }
        }
    }
}
