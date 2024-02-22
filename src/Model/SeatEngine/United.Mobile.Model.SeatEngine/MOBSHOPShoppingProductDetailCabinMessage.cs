using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable]
    public class MOBSHOPShoppingProductDetailCabinMessage
    {


        private string Segments = string.Empty;
        public string segments
        {
            get { return Segments; }
            set { Segments = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string cabin = string.Empty;
        public string Cabin
        {
            get { return cabin; }
            set { cabin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool isMixedCabin = false;
        public bool IsMixedCabin
        {
            get { return isMixedCabin; }
            set { isMixedCabin = value; }
        }
    }
}
