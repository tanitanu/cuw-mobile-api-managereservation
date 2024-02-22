using System;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBDestinationImageOnTop
    {
        private string imageUrl = string.Empty;
        private string imageDesc = string.Empty;
        private string nickName = string.Empty;

        public string ImageUrl
        {
            get
            {
                return this.imageUrl;
            }
            set
            {
                this.imageUrl = value;
            }
        }

        public string ImageDesc
        {
            get
            {
                return this.imageDesc;
            }
            set
            {
                this.imageDesc = value;
            }
        }

        public string NickName
        {
            get
            {
                return this.nickName;
            }
            set
            {
                this.nickName = value;
            }
        }
    }
}
