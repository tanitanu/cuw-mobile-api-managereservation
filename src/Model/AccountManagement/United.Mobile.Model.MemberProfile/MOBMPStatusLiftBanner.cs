using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    public class MOBMPStatusLiftBanner
    {
        private string imageSrcURL;
        private string premierStatusURL;

        public string ImageSrcURL
        {
            get
            {
                return this.imageSrcURL;
            }
            set
            {
                this.imageSrcURL = value;
            }
        }

        public string PremierStatusURL
        {
            get
            {
                return this.premierStatusURL;
            }
            set
            {
                this.premierStatusURL = value;
            }
        }

    }
}
