using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBSHOPFlattenedFlightList
    {
        public string ObjectName { get; set; } = "United.Persist.Definition.Shopping.MOBSHOPFlattenedFlightList";

        private List<MOBSHOPFlattenedFlight> flattenedFlightList;

        public List<MOBSHOPFlattenedFlight> FlattenedFlightList
        {
            get { return flattenedFlightList; }
            set { flattenedFlightList = value; }
        }

    }
}
