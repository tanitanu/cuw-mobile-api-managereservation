using System;
using System.Collections.Generic;

namespace United.Mobile.Model.Internal.HomePageContent
{
    public class NewsDetail
    {
        public string GetsMSG { get; set; }
        public string NewsID { get; set; }
        public string HeadingRed { get; set; }
        public string HeadingBlack { get; set; }
        public object SpaceCount { get; set; }
        public DateTime PostDate { get; set; }
        public string PostBody { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public DateTime UpdateDate { get; set; }
        public string EmployeeId { get; set; }
        public List<string> EmployeeStatus { get; set; }
        public List<string> WorkGroup { get; set; }
    }
}
