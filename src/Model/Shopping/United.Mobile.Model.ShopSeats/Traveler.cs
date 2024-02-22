using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ShopSeats
{
    public class Traveler
    {
        public virtual string LastName { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string ShareNameIndex { get; set; }
        public virtual string FQTVNumber { get; set; }
        public virtual string FQTVCarrier { get; set; }
        public virtual int EliteLevel { get; set; }
        public virtual string TicketNumber { get; set; }
        public virtual bool IsSelected { get; set; }
        public virtual bool IsEPlusSubscriber { get; set; }
        public virtual int NumberOfCompanion { get; set; }

        public virtual Collection<SeatInfo> Seats { get; set; }
        public virtual Collection<SeatInfo> OldSeats { get; set; }
        public virtual Collection<Entitlement> Entitlements { get; set; }
    }
}
