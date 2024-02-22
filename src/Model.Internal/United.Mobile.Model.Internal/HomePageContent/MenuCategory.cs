using System.Collections.Generic;

namespace United.Mobile.Model.Internal.HomePageContent
{
    public class MenuCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int ParentMenuId { get; set; }
        public List<SubMenu> SubMenu { get; set; }
    }
}
