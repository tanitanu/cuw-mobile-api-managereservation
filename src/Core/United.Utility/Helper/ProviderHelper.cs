using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace United.Utility.Helper
{
    public static class ProviderHelper
    {
        # region ToFromBinary

        public static byte[] ToBinary(Object oObject)
        {
            var formatter = new BinaryFormatter();
            using (var binaryStream = new MemoryStream())
            {
                formatter.Serialize(binaryStream, oObject);
                return binaryStream.ToArray();
            }
        }

        public static T FromBinary<T>(Byte[] bytes)
        {
            var formatter = new BinaryFormatter();
            using (var binaryStream = new MemoryStream(bytes))
            {
                var log = formatter.Deserialize(binaryStream);
                return (T)log;
            }
        }

        # endregion

        # region XMLJson Serialize Methods

        public static List<T> DeserializeListAll<T>(string input)
        {
            List<T> deserializedList = null;
            var serializeType = IsXmlOrJson(input);
            if (serializeType == SerializeType.Xml)
            {
                deserializedList = DeserializeXmlToList<T>(input);
            }

            if (serializeType == SerializeType.Json)
            {
                deserializedList = DeserializeJsonToList<T>(input);
            }
            return deserializedList;
        }

        public static List<T> DeserializeXmlToList<T>(string input)
        {
            if (string.IsNullOrEmpty(input)) return null;

            var doc = XDocument.Parse(input);
            var serializer = new DataContractSerializer(typeof(List<T>));
            using (var reader = doc.CreateReader())
            {
                var result = (List<T>)serializer.ReadObject(reader);
                return result;
            }
        }

        public static List<T> DeserializeJsonToList<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(List<T>));
                return (List<T>)serializer.ReadObject(ms);
            }
        }

        public static string SerializeAll(object obj, SerializeType serializeType)
        {
            var serializedString = string.Empty;
            if (serializeType == SerializeType.Xml)
            {
                serializedString = SerializeXml(obj);
            }

            if (serializeType == SerializeType.Json)
            {
                serializedString = SerializeJson(obj);
            }
            return serializedString;
        }

        public static T DeserializerAll<T>(string inputString)
        {
            T defObj = default(T);
            if (IsXmlOrJson(inputString) == SerializeType.Xml)
            {
                defObj = DeserializeXml<T>(inputString);
            }
            if (IsXmlOrJson(inputString) == SerializeType.Json)
            {
                defObj = DeserializeJson<T>(inputString);
            }
            return defObj;
        }

        public static SerializeType IsXmlOrJson(string input)
        {
            SerializeType stringType = SerializeType.Xml;

            if (string.IsNullOrEmpty(input)) return stringType;

            input = input.Trim();

            if (input.StartsWith("<") && input.EndsWith(">"))
            {
                stringType = SerializeType.Xml;
                return stringType;
            }

            if (input.StartsWith("{") && input.EndsWith("}") || input.StartsWith("[") && input.EndsWith("]"))
            {
                stringType = SerializeType.Json;
                return stringType;
            }

            return stringType;
        }

        public static string SerializeXml(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                var serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public static object DeserializeXml(string xml, Type toType)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var quotas = new XmlDictionaryReaderQuotas();
                quotas.MaxStringContentLength = 50000000;
                using (var reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, quotas, null))
                {
                    var serializer = new DataContractSerializer(toType);
                    return serializer.ReadObject(reader);
                }
            }
        }

        public static T DeserializeXml<T>(string xml)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
            {
                var quotas = new XmlDictionaryReaderQuotas();
                quotas.MaxStringContentLength = 50000000;
                using (var reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, quotas, null))
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    var obj = (T)serializer.ReadObject(reader);
                    return obj;
                }
            }
        }

        public static string SerializeJson(object obj)
        {
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    var serializer = new DataContractJsonSerializer(obj.GetType());
                    serializer.WriteObject(memoryStream, obj);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return string.Empty;
        }


        public static object DeserializeJson(string json, Type toType)
        {
            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    var serializer = new DataContractJsonSerializer(toType);
                    return serializer.ReadObject(ms);
                }
            }
        }

        public static T DeserializeJson<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var obj = (T)serializer.ReadObject(ms);
                return obj;
            }
        }

        public static byte[] Serialize(object obj)
        {

            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, obj);

                return ms.ToArray();
            }
        }

        public static object DeSerialize(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                var bf = new BinaryFormatter();
                return bf.Deserialize(ms);
            }
        }

        public static string ToXml<T>(T oObject)
        {
            var xmlDoc = new XmlDocument();
            var xmlSerializer = new XmlSerializer(oObject.GetType());
            using (var xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, oObject);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);

                return xmlDoc.InnerXml;
            }
        }

        public static string ToJson<T>(T data)
        {
            try
            {
                var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(data, Formatting.Indented,
                                                new Newtonsoft.Json.JsonSerializerSettings
                                                {
                                                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
                                                });
                return jsonString;
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
            return string.Empty;
        }

        public static T FromJson<T>(string data)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(data, new Newtonsoft.Json.JsonSerializerSettings
            {
                TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All
            });
        }

        public static string XmlToJson(string xmlDoc)
        {
            var doc = new XmlDocument();
            doc.LoadXml(CleanXml(xmlDoc));
            var jsonDoc = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc, Formatting.Indented);
            //var jsonString = jsonDoc.Replace("null", "[]");
            return jsonDoc;
            //var cleanedJson = CleanJson(jsonDoc);
            //return cleanedJson;
        }

        # endregion

        # region CleanupMethods

        public static string CleanXml(string xmlDoc)
        {
            const string namespacePattern = @"\sxmlns[^""]+""[^""]+""";
            const string prefixPattern = @"((?<=</?)\w+:(?<elem>\w+)|\w+:(?<elem>\w+)(?==\""))";

            //Replaces all namespaces with empty string
            var namespaceCleaned = Regex.Replace(xmlDoc, namespacePattern, string.Empty);

            //Removes all tag prefixes from prevoiusly cleaned string and returns clean xml
            return Regex.Replace(namespaceCleaned, prefixPattern, "${elem}");
        }

        public static string CleanResponseForParsing(string data)
        {
            const string namespacePattern = @"\sxmlns[^""]+""[^""]+""";
            const string prefixPattern = @"((?<=</?)\w+:(?<elem>\w+)|\w+:(?<elem>\w+)(?==\""))";

            //Replaces all namespaces with empty string
            var namespaceCleaned = Regex.Replace(data, namespacePattern, string.Empty);
            //Removes all tag prefixes from prevoiusly cleaned string and returns clean xml
            var cleaned = Regex.Replace(namespaceCleaned, prefixPattern, "${elem}");
            cleaned = Regex.Replace(cleaned, @"\s+", "");
            var cleanedData = cleaned.Replace(@"""<", "<")
                                                .Replace(@">""", ">")
                                                .Replace("\\n", "")
                                                .Replace("\\r", "")
                                                .Replace("&lt;", "<")
                                                .Replace("&gt;", ">")
                                                .Replace(@"\""", @"""")
                                                .Replace(@"\\", "")
                                                .Replace("'", "");
            var extraCharactersCleaned =
                cleanedData.Replace("\\n", "")
                       .Replace("\\r", "")
                       .Replace("\\\"", "'")
                       .Replace("\\", "")
                       .Replace("\"", "'")
                       .Replace("''", "")
                       .Replace("['{", "[{")
                       .Replace("}']", "}]")
                       .Replace("\"{", "{")
                       .Replace("}\"", "}")
                       .Replace("}'", "}")
                       .Replace("'{", "{");


            return extraCharactersCleaned; //result;
        }

        private static string CleanJson(string request)
        {
            const string xmlSeq = ":" + "\"" + "<";
            const string xmlSeq2 = "<.";
            if (request.Contains(xmlSeq) || request.Contains(xmlSeq2) || request.Contains("< .") ||
                 request.Contains("<?xml version"))
            {
                return request;
            }

            var singleQuoteRemoved = request.Replace("'", "");
            var cleanedup =
                singleQuoteRemoved.Replace("\\n", "")
                       .Replace("\\r", "")
                       .Replace("\\\"", "'")
                       .Replace("\\", "")
                       .Replace("\"", "'")
                       .Replace("''", "")
                       .Replace("['{", "[{")
                       .Replace("}']", "}]")
                       .Replace("\"{", "{")
                       .Replace("}\"", "}")
                       .Replace("}'", "}")
                       .Replace("'{", "{")
                       .Replace(":}", ":null}");
            try
            {
                dynamic deRequest = Newtonsoft.Json.JsonConvert.DeserializeObject(cleanedup);
                return deRequest.ToString();
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return request;
            }
        }
        #endregion

        # region Compression methods

        public static byte[] SerializeAndCompress(object obj)
        {
            using (var ms = new MemoryStream())
            {
                using (var zs = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    var bf = new BinaryFormatter();
                    bf.Serialize(zs, obj);
                }
                return ms.ToArray();
            }
        }

        public static object DecompressAndDeserialze(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var zs = new GZipStream(ms, CompressionMode.Decompress, true))
                {
                    var bf = new BinaryFormatter();
                    return bf.Deserialize(zs);
                }
            }
        }

        public static string Compress(string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public static Byte[] CompressToBinary(string s)
        {
            var bytes = Encoding.Unicode.GetBytes(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }
                return mso.ToArray();
            }
        }

        public static byte[] CompressBinary(byte[] raw)
        {
            using (var memory = new MemoryStream())
            {
                using (var gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }

        public static string Decompress(string s)
        {
            var bytes = Convert.FromBase64String(s);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }

        public static byte[] Decompress(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return mso.ToArray();
            }
        }

        public static string DecompressFromBinary(Byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    gs.CopyTo(mso);
                }
                return Encoding.Unicode.GetString(mso.ToArray());
            }
        }

        # endregion

    }
   
}
