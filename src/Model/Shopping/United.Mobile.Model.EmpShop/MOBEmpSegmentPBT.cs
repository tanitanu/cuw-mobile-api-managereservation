
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    /* PBT(Passengers Boarding Total)  for each Segment */
    [Serializable()]
    public class MOBEmpSegmentPBT : MOBResponse 
    {
       
        private MOBEmpPBTType capacity;
        private MOBEmpPBTType available;
        private MOBEmpPBTType authorized;
        private MOBEmpPBTType booked;
        private MOBEmpPBTType revenueStandBy;
        private MOBEmpPBTType held;
        private MOBEmpPBTType positiveSpace;
        private MOBEmpPBTType spaceAvailable;
        private MOBEmpPBTType group;
        private MOBEmpPBTType checkedIn;
        private MOBEmpPBTType upgradableElite;

        public MOBEmpPBTType CheckedIn
        {
            get { return this.checkedIn; }
            set { this.checkedIn = value; }
        }

        public MOBEmpPBTType UpgradableElite
        {
            get { return this.upgradableElite; }
            set { this.upgradableElite = value; }
        }

        public MOBEmpPBTType Capacity
        {
            get { return this.capacity; }
            set { this.capacity = value; }
        }

        public MOBEmpPBTType Available
        {
            get { return this.available; }
            set { this.available = value; }
        }

        public MOBEmpPBTType Authorized
        {
            get { return this.authorized; }
            set { this.authorized = value; }
        }

        public MOBEmpPBTType Booked
        {
            get { return this.booked; }
            set { this.booked = value; }
        }

        public MOBEmpPBTType RevenueStandBy
        {
            get { return this.revenueStandBy; }
            set { this.revenueStandBy = value; }
        }

        public MOBEmpPBTType Held
        {
            get { return this.held; }
            set { this.held = value; }
        }

        public MOBEmpPBTType PositiveSpace
        {
            get { return this.positiveSpace; }
            set { this.positiveSpace = value; }
        }

        public MOBEmpPBTType SpaceAvailable
        {
            get { return this.spaceAvailable; }
            set { this.spaceAvailable = value; }
        }

        public MOBEmpPBTType Group
        {
            get { return this.group; }
            set { this.group = value; }
        }
    }
}
