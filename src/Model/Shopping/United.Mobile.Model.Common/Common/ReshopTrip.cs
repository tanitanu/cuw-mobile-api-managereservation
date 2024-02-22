using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    [XmlType(TypeName = "MOBSHOPReshopTrip")]
    public class ReshopTrip
    {
        private string changeTripTitle;
        public MOBSHOPTrip OriginalTrip { get; set; }

        public bool IsReshopTrip { get; set; }

        public string ChangeTripTitle
        {
            get
            {
                return this.changeTripTitle;
            }
            set
            {
                this.changeTripTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBSHOPTrip ChangeTrip { get; set; }

        public bool IsUsed { get; set; }

        public int OriginalUsedIndex { get; set; }
    }
}
