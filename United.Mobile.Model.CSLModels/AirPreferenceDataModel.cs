using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class AirPreferenceDataModel
    {
            public int AirPreferenceId
            {
                get;
                set;
            }

            public string AirportNameShort
            {
                get;
                set;
            }

            public string AirportNameLong
            {
                get;
                set;
            }

            public string MealCode
            {
                get;
                set;
            }


            public string MealDescription
            {
                get;
                set;
            }




            public string SeatSideDescription
            {
                get;
                set;
            }



            public int ClassID
            {
                get;
                set;
            }




            public string ClassDescription
            {
                get;
                set;
            }



            public int EquipmentID
            {
                get;
                set;
            }




            public string EquipmentDescription
            {
                get;
                set;
            }




            public string EquipmentCode
            {
                get;
                set;
            }



            public int SearchPreferenceID
            {
                get;
                set;
            }




            public string SearchPreferenceDescription
            {
                get;
                set;
            }



            public int NumberOfFlightsDisplay
            {
                get;
                set;
            }



            public int MealId
            {
                get;
                set;
            } = 1;




            public int SeatSide
            {
                get;
                set;
            }




            public string AirportCode
            {
                get;
                set;
            } = string.Empty;





            public string InsertID
            {
                get;
                set;
            }


            public string UpdateID
            {
                get;
                set;
            }

        public string Key
        {
            get;
            set;
        }
        public int VendorId { get; set; }
        public string VendorCode { get; set; }
        public string VendorDescription { get; set; }
    }
    
}
