using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class PrefServiceAnimal
    {
        public int AirPreferenceId { get; set; } 

        public int ServiceAnimalId { get; set; } 
        public string ServiceAnimalIdDesc { get; set; } 

        public int ServiceAnimalTypeId { get; set; } 
        public string ServiceAnimalTypeIdDesc { get; set; } = string.Empty;
      
        public string Key { get; set; } = string.Empty;
       
        public int Priority { get; set; }

        public bool IsNew { get; set; } 

        public bool IsSelected { get; set; } 
    }
}
