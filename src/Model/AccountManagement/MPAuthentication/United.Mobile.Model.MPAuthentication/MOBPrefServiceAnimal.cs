using System;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable]
    public class MOBPrefServiceAnimal
    {
        private int airPreferenceId;
        private int serviceAnimalId;
        private string serviceAnimalIdDesc;
        private int serviceAnimalTypeId;
        private string serviceAnimalTypeIdDesc;
        private string key = string.Empty;
        private int priority;
        private bool isNew;
        private bool isSelected;

        public int AirPreferenceId
        {
            get
            {
                return airPreferenceId;
            }
            set
            {
                airPreferenceId = value;
            }
        }

        public int ServiceAnimalId
        {
            get { return serviceAnimalId; }
            set { serviceAnimalId = value; }
        }

        public string ServiceAnimalIdDesc
        {
            get { return serviceAnimalIdDesc; }
            set { serviceAnimalIdDesc = value; }
        }

        public int ServiceAnimalTypeId
        {
            get { return serviceAnimalTypeId; }
            set { serviceAnimalTypeId = value; }
        }

        public string ServiceAnimalTypeIdDesc
        {
            get { return serviceAnimalTypeIdDesc; }
            set { serviceAnimalTypeIdDesc = value; }
        }

        public string Key
        {
            get
            {
                return this.key;
            }
            set
            {
                this.key = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public int Priority
        {
            get
            {
                return priority;
            }
            set
            {
                priority = value;
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
    }
}
