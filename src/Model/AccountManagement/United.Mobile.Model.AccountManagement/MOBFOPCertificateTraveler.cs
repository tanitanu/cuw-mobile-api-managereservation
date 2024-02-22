using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBFOPCertificateTraveler
    {
        private string travelerNameIndex;
        private string name;
        private bool isCertificateApplied;
        private double individualTotalAmount;
        private int paxId;

        public int PaxId
        {
            get { return paxId; }
            set { paxId = value; }
        }


        public double IndividualTotalAmount
        {
            get { return individualTotalAmount; }
            set { individualTotalAmount = value; }
        }


        public bool IsCertificateApplied
        {
            get { return isCertificateApplied; }
            set { isCertificateApplied = value; }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string TravelerNameIndex
        {
            get { return travelerNameIndex; }
            set { travelerNameIndex = value; }
        }



    }
}
