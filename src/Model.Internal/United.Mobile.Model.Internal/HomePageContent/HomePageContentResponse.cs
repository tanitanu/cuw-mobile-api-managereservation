using System.Collections.Generic;
using United.Mobile.Model.Internal.Common;

namespace United.Mobile.Model.Internal.HomePageContent
{
    public class HomePageContentResponse :EResBaseResponse
    {        
        public Content Content { get; set; }
        public List<NewsDetail> NewsDetails { get; set; }
        public List<MOBTravelType> TravelType { get; set; }
        public string EmployeeMsgType { get; set; }
        public List<EResAlert> BaseAlert { get; set; }
        public List<Menu> Menus { get; set; }       
    }
}

