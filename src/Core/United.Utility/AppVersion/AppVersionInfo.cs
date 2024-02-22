using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using United.Mobile.Model;

namespace United.Utility.AppVersion
{
    public class AppVersionInfo : IAppVersionInfo
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string AppMajorVersion;
        private readonly int AppID;

        public AppVersionInfo(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            AppMajorVersion = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMajorText].ToString();
            AppID = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppIdText]);
        }

        public bool IsGreaterorEqual(string iOSConfigEntryName, string androidConfigEntryName)
        {
            string configAppVersion = (AppID == 1) ?
                _configuration.GetValue<string>(iOSConfigEntryName) :
                _configuration.GetValue<string>(androidConfigEntryName);

            return VerifyIsGreaterorEqual(AppMajorVersion, configAppVersion);
        }

        public bool IsGreater(string iOSConfigEntryName, string androidConfigEntryName)
        {
            string configAppVersion = (AppID == 1) ?
                _configuration.GetValue<string>(iOSConfigEntryName) :
                _configuration.GetValue<string>(androidConfigEntryName);

            return VerifyIsGreater(AppMajorVersion, configAppVersion);
        }

        public bool IsGreaterorEqual(string sectionName)
        {
            var configsectionAppVersion = _configuration.GetSection(sectionName).Get<AppVersion>();
            string configAppVersion = (AppID == 1) ?
                configsectionAppVersion.iOS :
                configsectionAppVersion.Android;

            return VerifyIsGreaterorEqual(AppMajorVersion, configAppVersion);
        }

        public bool IsGreater(string sectionName)
        {

            var configsectionAppVersion = _configuration.GetSection(sectionName).Get<AppVersion>();
            string configAppVersion = (AppID == 1) ?
                configsectionAppVersion.iOS :
                configsectionAppVersion.Android;
            return VerifyIsGreater(AppMajorVersion, configAppVersion);

        }

        public bool IsGreaterorEqual(AppVersion configSectionAppVersion)
        {
            if (configSectionAppVersion == null)
                return false;

            string configAppVersion = (AppID == 1) ?
                configSectionAppVersion.iOS :
                configSectionAppVersion.Android;

            return VerifyIsGreaterorEqual(AppMajorVersion, configAppVersion);
        }

        public bool IsGreater(AppVersion configSectionAppVersion)
        {
            if (configSectionAppVersion == null)
                return false;

            string configAppVersion = (AppID == 1) ?
                configSectionAppVersion.iOS :
                configSectionAppVersion.Android;

            return VerifyIsGreater(AppMajorVersion, configAppVersion);
        }

        private bool VerifyIsGreater(string appVersion, string configAppVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && !string.IsNullOrEmpty(configAppVersion))
            {
                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());

                System.Version localVersion1;
                System.Version localVersion2;
                if (System.Version.TryParse(appVersion, out localVersion1) &&
                    System.Version.TryParse(configAppVersion, out localVersion2))
                {
                    return localVersion1 > localVersion2;
                }
            }

            return false;
        }

        private bool VerifyIsGreaterorEqual(string appVersion, string configAppVersion)
        {

            if (!string.IsNullOrEmpty(appVersion) && !string.IsNullOrEmpty(configAppVersion))
            {
                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());

                System.Version localVersion1;
                System.Version localVersion2;
                if (System.Version.TryParse(appVersion, out localVersion1) &&
                    System.Version.TryParse(configAppVersion, out localVersion2))
                {
                    return localVersion1 >= localVersion2;
                }
            }

            return false;
        }


        private bool VerifyIsLesserorEqual(string appVersion, string configAppVersion)
        {

            if (!string.IsNullOrEmpty(appVersion) && !string.IsNullOrEmpty(configAppVersion))
            {
                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());

                System.Version localVersion1;
                System.Version localVersion2;
                if (System.Version.TryParse(appVersion, out localVersion1) &&
                    System.Version.TryParse(configAppVersion, out localVersion2))
                {
                    return localVersion1 <= localVersion2;
                }
            }

            return false;
        }


        public bool IsLesserorEqual(string iOSConfigEntryName, string androidConfigEntryName)
        {
            string configAppVersion = (AppID == 1) ?
                _configuration.GetValue<string>(iOSConfigEntryName) :
                _configuration.GetValue<string>(androidConfigEntryName);

            return VerifyIsLesserorEqual(AppMajorVersion, configAppVersion);
        }
    }
}
