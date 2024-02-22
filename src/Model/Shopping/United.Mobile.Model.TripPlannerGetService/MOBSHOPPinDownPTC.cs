using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace United.Mobile.Model.TripPlannerGetService
{
    [Serializable]
    public class MOBSHOPPinDownPTC
    {
        private string type = string.Empty;
        private int count;

        [XmlAttribute]
        public string Type
        {
            get
            {
                return this.type;
            }
            set
            {
                this.type = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        [XmlAttribute]
        public int Count
        {
            get
            {
                return this.count;
            }
            set
            {
                this.count = value;
            }
        }
    }
}
