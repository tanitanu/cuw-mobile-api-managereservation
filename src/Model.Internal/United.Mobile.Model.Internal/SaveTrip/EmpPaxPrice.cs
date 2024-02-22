using System.Collections.Generic;

namespace United.Mobile.Model.Internal.SaveTrip
{
    public class EmpPaxPrice
    {
        public string BaseFare { get; set; }
        public decimal RawBaseFare { get; set; }
        public List<EmpTax> Taxes { get; set; }
        public string TotalFare { get; set; }
        public decimal TotalFareRaw { get; set; }
        public List<string> Errors { get; set; }
        public string Destination { get; set; }
    }

}
