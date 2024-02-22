using System;
using System.Collections.Generic;
using System.Text;

namespace United.Utility.AppVersion
{
    public interface IAppVersionInfo
    {
        public bool IsGreaterorEqual(string iOSConfigEntryName, string androidConfigEntryName);
        public bool IsGreater(string iOSConfigEntryName, string androidConfigEntryName);

        public bool IsGreaterorEqual(string sectionName);
        public bool IsGreater(string sectionName);

        public bool IsGreaterorEqual(AppVersion configSectionAppVersion);
        public bool IsGreater(AppVersion configSectionAppVersion);

        public bool IsLesserorEqual(string iOSConfigEntryName, string androidConfigEntryName);

    }
}
