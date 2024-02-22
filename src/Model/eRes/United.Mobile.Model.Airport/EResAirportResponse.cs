using System.Collections.Generic;

namespace United.Mobile.Model.Airport
{
    public class EResAirportResponse
    {
        public List<EResAirportDetail> Stations { get; set; }
    }
    public class EResAirportDetail
    {
        public string CityCode { get; set; }
        public string Code { get; set; }
        public string MName { get; set; }//AirportNameMobile
        public string SName { get; set; } //AirportNameShort
        public string City { get; set; }
        public string Country { get; set; }
        public int AllAirportFlag { get; set; }
    }
}
