using System.Collections.Generic;
namespace United.Mobile.Model.Internal.EmployeeProfile
{
    public class MileagePlusRequest
    {
        public string DataSetting { get; set; } = null;
        public List<string> DataToLoad { get; set; }
        public string LangCode { get; set; } = null;
        public string MileagePlusId { get; set; } = null;
        public List<int> MemberCustomerIdsToLoad { get; set; }
        public List<string> MemberTravelerKeysToLoad { get; set; }
        public string InsertId { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MPPwd { get; set; } = string.Empty;
        public string UpdateId { get; set; } = string.Empty;
        public string SidaId { get; set; } = string.Empty;
        public string SidaLocationCode { get; set; } = string.Empty;
        public string SidaExpirationDate { get; set; } = string.Empty;
    }
}
