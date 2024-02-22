using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Travelers
{
    [Serializable()]
   public class AZUREPlacePass
    {
            private string destination;
            private string propertyDomainUrl;
            private string mobileImageUrl;
            private string txtPoweredBy;
            private string txtPlacepass;
            private PlacePassLocation nearestLocation;
            private bool validLocation;

        public bool ValidLocation
        {
            get { return validLocation; }
            set { validLocation = value; }
        }


        public PlacePassLocation NearestLocation 
            {
                get { return nearestLocation; }
                set { nearestLocation = value; }
            }
            public string Destination
            {
                    get { return destination; }
                    set { destination = value; }
            }
            public string TxtPoweredBy
            {
                    get { return this.txtPoweredBy; }
                    set { this.txtPoweredBy = value; }
            }

            public string TxtPlacepass
            {
                get { return this.txtPlacepass; }
                set { this.txtPlacepass = value; }
            }
            public string PropertyDomainUrl
            {
                get
                {
                    return this.propertyDomainUrl;
                }
                set
                {
                    this.propertyDomainUrl = value;
                }
            }
            public string MobileImageUrl
            {
                get
                {
                    return this.mobileImageUrl;
                }
                set
                {
                    this.mobileImageUrl = value;
                }
            }
        }

    public class PlacePassLocation
    {
        private string City;

        public string city
        {
            get { return City; }
            set { City = value; }
        }

    }
    }
