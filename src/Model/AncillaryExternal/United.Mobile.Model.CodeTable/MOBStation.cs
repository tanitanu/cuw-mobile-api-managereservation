using System;

namespace United.Mobile.Model.CodeTable
{
    [Serializable()]
    public class MOBStation
    {
        public int SortOrder { get; set; } 

        public string Code { get; set; } = string.Empty;

        public string SName { get; set; } = string.Empty;

        public string MName { get; set; } = string.Empty;

        public string AFlag { get; set; } = string.Empty;

        public string CityCode { get; set; } = string.Empty;

        public int AllAirportFlag { get; set; } 

    }
}
