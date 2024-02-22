namespace United.Mobile.Model.Internal.HomePageContent
{
    public class SubMenu
    {
        public int SubMenuId { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public bool IsExternalLink { get; set; }
        public int MenuCategoryId { get; set; }
        public bool IsExternalSite { get; set; }
    }
}
