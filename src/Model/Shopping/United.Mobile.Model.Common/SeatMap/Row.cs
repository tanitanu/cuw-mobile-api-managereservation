using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBRow
    {
        public MOBRow() { }

        public MOBRow(List<MOBSeatB> seats, string number, bool wing)
        {
            Number = number;
            Seats = seats;
            Wing = wing;
        }

        private List<MOBSeatB> seats = new List<MOBSeatB>();
        public List<MOBSeatB> Seats
        {
            get { return seats; }
            set { seats = value; }
        }

        private string number;
        public string Number
        {
            get { return number; }
            set { number = value; }
        }

        private bool wing;
        public bool Wing
        {
            get { return wing; }
            set { wing = value; }
        }
    }
}
