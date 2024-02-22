using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class Option
    {
       
        public string OptionIcon { get; set; } = string.Empty;

        public string OptionDescription { get; set; } = string.Empty;

        public bool AvailableInElf { get; set; } 

        public bool AvailableInEconomy { get; set; }


        /// <summary>
        /// Added since IBELite implementation in order to be about to show prices in the confirm fare screen
        /// </summary>
        private string FareSubDescriptionELF;
        public string fareSubDescriptionELF
        {
            get { return FareSubDescriptionELF; }
            set { FareSubDescriptionELF = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }


        /// <summary>
        /// Added since IBELite implementation in order to be about to show prices in the confirm fare screen
        /// </summary>
        private string FareSubDescriptionEconomy;
        public string fareSubDescriptionEconomy
        {
            get { return FareSubDescriptionEconomy; }
            set { FareSubDescriptionEconomy = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }
}
