using System;

namespace United.Mobile.Model.FlightStatus
{
    [Serializable]
    public class TypeOption
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public TypeOption(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public TypeOption()
        { }
    }
}