using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;

namespace United.Mobile.Model.Common
{
    public class EnumCommon
    {
        public string Title { get; set; }

        //[System.Text.Json.Serialization.JsonConverter(typeof(JsonStringEnumConverter))]
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public DisplayGendre Genre { get; set; }
    }

    public enum DisplayGendre
    {
        [EnumMember(Value = "0")]
        MALE,
        [EnumMember(Value = "1")]
        FEMALE
    }
}
