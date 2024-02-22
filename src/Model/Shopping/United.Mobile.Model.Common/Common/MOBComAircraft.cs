using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBComAircraft
    {
        //TODO Sathwika
        private int cabinCount;
       // private List<MOBComCabin> cabins;
        //private ComAircraftModel model;
        private string noseNumber = string.Empty;
        private string ownerAirlineCode = string.Empty;
        private string shipNumber = string.Empty;
        private string tailNumber = string.Empty;

        public int CabinCount
        {
            get
            {
                return this.cabinCount;
            }
            set
            {
                this.cabinCount = value;
            }
        }

        //public List<ComCabin> Cabins
        //{
        //    get
        //    {
        //        return this.cabins;
        //    }
        //    set
        //    {
        //        this.cabins = value;
        //    }
        //}

        //public ComAircraftModel Model
        //{
        //    get
        //    {
        //        return this.model;
        //    }
        //    set
        //    {
        //        this.model = value;
        //    }
        //}

        public string NoseNumber
        {
            get
            {
                return this.noseNumber;
            }
            set
            {
                this.noseNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string OwnerAirlineCode
        {
            get
            {
                return this.ownerAirlineCode;
            }
            set
            {
                this.ownerAirlineCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string ShipNumber
        {
            get
            {
                return this.shipNumber;
            }
            set
            {
                this.shipNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TailNumber
        {
            get
            {
                return this.tailNumber;
            }
            set
            {
                this.tailNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
