using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBCabin
    {
        public MOBCabin() { }
        public MOBCabin(List<MOBRow> rows, string cos)
        {
            Rows = rows;
            COS = cos;
        }

        private List<MOBRow> rows = new List<MOBRow>();
        public List<MOBRow> Rows
        {
            get { return rows; }
            set { rows = value; }
        }

        private string cos = string.Empty;
        public bool HasAvailableSeats { get; set; }
        public bool HasEnoughPcuSeats { get; set; }
        public string PcuOptionId { get; set; }
        public string COS
        {
            get { return cos; }
            set { cos = value; }
        }

        private string configuration;
        public string Configuration
        {
            get { return configuration; }
            set { configuration = value; }
        }

        public List<MOBRow> FrontMonuments { get; set; }
        public List<MOBRow> BackMonuments { get; set; }


    }
}
