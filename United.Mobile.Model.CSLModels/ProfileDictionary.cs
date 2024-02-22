using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class ProfileDictionary
    {
        #region Constructors

        public ProfileDictionary(string key, string value)
        {
            Key = key;
            Value = value;
        }

        #endregion Constructors

        #region Properties

        public string Key { get; set; }
        public string Value { get; set; }

        #endregion Properties
    }
}
