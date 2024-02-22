using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.PassRiderList
{
    public class EmpStandByListRequest
    {
        public string FlightNumber { get; set; }
        public string FlightDate { get; set; }
        public string Departure { get; set; }
        public string Equipment { get; set; }
        public string EquipmentDesc { get; set; }
        public bool ShowPosition { get; set; }
        public string Destination { get; set; }
        public string Origin { get; set; }
        public string EmployeeId { get; set; }
        public bool IsGetPBT { get; set; }
        public string HashPinCode { get; set; }
        public string MileagePlusNumber { get; set; }
        public string SessionId { get; set; }
        public string CarrierCode { get; set; }
        public string CarrierDescritption { get; set; }
        public string OperatingCarrier { get; set; }
        public string TokenId { get; set; }

    }
}