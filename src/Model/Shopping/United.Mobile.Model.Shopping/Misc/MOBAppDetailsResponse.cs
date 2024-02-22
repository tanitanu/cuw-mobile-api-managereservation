using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping.Misc
{
    [Serializable]
    public class AppDetailsResponse :MOBResponse
    {
        private string requestUrl = string.Empty;

        public string RequestUrl
        {
            get { return requestUrl; }
            set { requestUrl = value; }
        }
        private string version;

        public string Version
        {
            get { return version; }
            set { version = value; }
        }

        private string releaseNotes;

        public string ReleaseNotes
        {
            get { return releaseNotes; }
            set { releaseNotes = value; }
        }    
    }

    public class AppDetailsResult
    {
        public string kind { get; set; }
        public string[] features { get; set; }
        public string[] supportedDevices { get; set; }
        public object[] advisories { get; set; }
        public bool isGameCenterEnabled { get; set; }
        public string trackCensoredName { get; set; }
        public string[] languageCodesISO2A { get; set; }
        public string fileSizeBytes { get; set; }
        public string sellerUrl { get; set; }
        public string contentAdvisoryRating { get; set; }
        public float averageUserRatingForCurrentVersion { get; set; }
        public int userRatingCountForCurrentVersion { get; set; }
        public string trackViewUrl { get; set; }
        public string trackContentRating { get; set; }
        public string currency { get; set; }
        public string wrapperType { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public int artistId { get; set; }
        public string artistName { get; set; }
        public string[] genres { get; set; }
        public float price { get; set; }
        public string trackName { get; set; }
        public int trackId { get; set; }
        public string[] genreIds { get; set; }
        public DateTime releaseDate { get; set; }
        public string sellerName { get; set; }
        public string bundleId { get; set; }
        public string releaseNotes { get; set; }
        public string primaryGenreName { get; set; }
        public int primaryGenreId { get; set; }
        public bool isVppDeviceBasedLicensingEnabled { get; set; }
        public string minimumOsVersion { get; set; }
        public string formattedPrice { get; set; }
        public float averageUserRating { get; set; }
        public int userRatingCount { get; set; }
    }
}
