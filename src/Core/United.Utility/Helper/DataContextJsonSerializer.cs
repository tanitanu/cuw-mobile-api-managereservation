using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
namespace United.Utility.Helper
{
    public class DataContextJsonSerializer
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string Serialize<T>(T t)
        {
            try
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream memoryStream = new MemoryStream();
                dataContractJsonSerializer.WriteObject(memoryStream, t);
                string jsonString = Encoding.UTF8.GetString(memoryStream.ToArray());
                memoryStream.Close();
                return jsonString;
            }
            catch
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject(t);
            }
        }

        public static string SerializeToJSON<T>(T t)
        {
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream();
            dataContractJsonSerializer.WriteObject(memoryStream, t);
            string jsonString = Encoding.UTF8.GetString(memoryStream.ToArray());
            memoryStream.Close();
            return jsonString;
        }

        public static string NewtonSoftSerializeToJson<T>(T t)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(t);
        }
        //public static string Serialize(this object obj)
        //{
        //    System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //    //serializer.RegisterConverters(new JavaScriptConverter[] { new SectionConverter(), new SeatMapScreenConverter() });
        //    return serializer.Serialize(obj);
        //}

        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T Deserialize<T>(string strJSON)
        {
            try
            {

                //JavaScriptSerializer serializer = new JavaScriptSerializer();
                //serializer.RegisterConverters(new JavaScriptConverter[] { new SectionConverter(), new SeatMapScreenConverter() });
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strJSON);
            }
            catch
            {
                return DeserializeUseContract<T>(strJSON);
            }
        }

        public static T DeserializeUseContract<T>(string jsonString)
        {
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)dataContractJsonSerializer.ReadObject(memoryStream);
            return obj;
        }
        public static T DeserializeUseContract<T>(Stream responseStream)
        {
            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            T obj = (T)dataContractJsonSerializer.ReadObject(responseStream);
            return obj;
        }

        public static T NewtonSoftDeserialize<T>(string strJSON)
        {
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strJSON);
        }

        public static T DeserializeJsonDataContract<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var obj = (T)serializer.ReadObject(ms);
                return obj;
            }
        }

        public static T DeserializeFromJsonALL<T>(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data, new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            });
        }
        public static T NewtonSoftDeserializeIgnoreErrorAndReturnNull<T>(string strJSON)
        {
            // This method is returning not throw exception when input is empty or deserialization fails. It returns null instead. 
            if (!string.IsNullOrEmpty(strJSON))
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(strJSON, new JsonSerializerSettings()
                {
                    Error = (sender, error) => error.ErrorContext.Handled = true
                });
            }

            return default(T);
        }

    }

    public enum SerializeType
    {
        Xml,
        Json,
        String
    }
}
