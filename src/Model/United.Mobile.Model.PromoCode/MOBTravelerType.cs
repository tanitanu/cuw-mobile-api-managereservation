using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PromoCode
{
    [Serializable()]
    public class MOBTravelerType
    {
        private int count;
        private string travelerType;
        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        public string TravelerType
        {
            get
            {
                return travelerType;
            }
            set
            {
                travelerType = value;
            }
        }
    }

    [Serializable()]
    public class MOBDisplayTravelType
    {
        private int paxID;
        private string paxType;
        private string travelerDescription;
        private MOBPAXTYPE travelerType;

        public int PaxID
        {
            get { return paxID; }
            set { paxID = value; }
        }
        public string PaxType
        {
            get { return paxType; }
            set { paxType = value; }
        }
        public string TravelerDescription
        {
            get { return travelerDescription; }
            set { travelerDescription = value; }
        }
        public MOBPAXTYPE TravelerType
        {
            get { return travelerType; }
            set { travelerType = value; }
        }
    }

    public enum MOBPAXTYPE
    {
        Adult,
        Child2To4,
        Child5To11,
        Child12To17,
        InfantLap,
        InfantSeat,
        Senior,
        Child12To14,
        Child15To17
    }
}
