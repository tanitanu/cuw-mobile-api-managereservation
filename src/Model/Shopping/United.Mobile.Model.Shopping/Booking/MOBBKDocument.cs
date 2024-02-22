using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Booking
{
    [Serializable()]
    public class MOBBKDocument
    {
        private string documentId = string.Empty;
        private string knownTravelerNumber = string.Empty;
        private string redressNumber = string.Empty;

        public string DocumentId
        {
            get
            {
                return this.documentId;
            }
            set
            {
                this.documentId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string KnownTravelerNumber
        {
            get
            {
                return this.knownTravelerNumber;
            }
            set
            {
                this.knownTravelerNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RedressNumber
        {
            get
            {
                return this.redressNumber;
            }
            set
            {
                this.redressNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
