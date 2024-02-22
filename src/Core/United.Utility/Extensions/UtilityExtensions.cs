using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace United.Utility.Extensions
{
    public static class UtilityExtensions
    {
        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is System.Enum) 
            {
                Type type = e.GetType();
                Array values = System.Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return null; // could also return string.Empty
        }
    }

    public class TimeSpanContractResolver : DefaultContractResolver
    {
        // public new static readonly TimeSpanContractResolver Instance = new TimeSpanContractResolver();

        protected override JsonContract CreateContract(Type objectType)
        {
            JsonContract contract = base.CreateContract(objectType);

            // this will only be called once and then cached
            if (objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?))
            {
                contract.Converter = new TimeSpanConverter();
            }

            return contract;
        }
    }

    public class TimeSpanConverter : Newtonsoft.Json.JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ts = (TimeSpan)value;
            var str = XmlConvert.ToString(ts);
            serializer.Serialize(writer, str);
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
            try
            {
                var value = serializer.Deserialize<string>(reader);
                return XmlConvert.ToTimeSpan(value);
            }
            catch { }
            return existingValue;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }
    }
}
