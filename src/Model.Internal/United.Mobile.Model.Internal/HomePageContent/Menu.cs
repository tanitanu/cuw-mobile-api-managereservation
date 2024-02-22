using System.Collections.Generic;

namespace United.Mobile.Model.Internal.HomePageContent
{
    public class Menu
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public List<MenuCategory> MenuCategories { get; set; }
    }
}
