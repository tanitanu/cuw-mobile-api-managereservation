using System;

namespace United.Mobile.Model.TeaserPage
{
    [Serializable]
    public class MOBSHOPTeaserText
    {
        private string text = string.Empty;
        public string Text
        {
            get { return text; }
            set { text = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string icon = string.Empty;
        public string Icon
        {
            get { return icon; }
            set { icon = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string langCode = string.Empty;
        public string LangCode
        {
            get { return langCode; }
            set { langCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool isPrimary = false;
        public bool IsPrimary
        {
            get { return isPrimary; }
            set { isPrimary = value; }
        }

        private int sortIndex = 0;
        public int SortIndex
        {
            get { return sortIndex; }
            set { sortIndex = value; }
        }

        private string itemType = string.Empty;
        public string ItemType
        {
            get { return itemType; }
            set { itemType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }
    }

}
