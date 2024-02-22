using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBInCabinPet
    {
        public List<MOBItem> Messages { get; set; }

        public string InCabinPetLabel { get; set; } = string.Empty;

        public string InCabinPetRefText { get; set; } = string.Empty;

        public string InCabinPetRefValue { get; set; } = string.Empty;
    }
}
