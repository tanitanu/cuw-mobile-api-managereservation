using System;

namespace United.Mobile.Model.ManageRes
{
    [Serializable()]
    public class MOBSHOPOption
    {
        private string optionDescription = string.Empty;
        private bool availableInElf = false;
        private bool availableInEconomy = false;
        private string optionIcon = string.Empty;
        private string fareSubDescriptionELF;
        private string fareSubDescriptionEconomy;

        public string OptionIcon
        {
            get
            {
                return this.optionIcon;
            }
            set
            {
                this.optionIcon = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OptionDescription
        {
            get
            {
                return this.optionDescription;
            }

            set
            {
                this.optionDescription = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool AvailableInElf
        {
            get
            {
                return this.availableInElf;
            }

            set
            {
                this.availableInElf = value;
            }
        }

        public bool AvailableInEconomy
        {
            get
            {
                return this.availableInEconomy;
            }

            set
            {
                this.availableInEconomy = value;
            }
        }

        /// <summary>
        /// Added since IBELite implementation in order to be about to show prices in the confirm fare screen
        /// </summary>
        public string FareSubDescriptionELF
        {
            get { return fareSubDescriptionELF; }
            set { fareSubDescriptionELF = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        /// <summary>
        /// Added since IBELite implementation in order to be about to show prices in the confirm fare screen
        /// </summary>
        public string FareSubDescriptionEconomy
        {
            get { return fareSubDescriptionEconomy; }
            set { fareSubDescriptionEconomy = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }
}
